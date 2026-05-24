using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.Sabotage;
using _Works._JTH.Scripts.SO;
using _Works._PMS.Code.Event;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIPresenter : BasePresenter
    {
        [SerializeField] private GameObject redMarkPrefab;
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private EventChannelSO cameraChannel;
        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private StageInfoSO stageInfoSO;
        [SerializeField] private PlayerStatSO playerStat;
        
        private InGameUIModel _inGameModel;
        private InGameUIView _inGameView;

        private List<Sabotage> _sabotageList;
        private int _sceneSabotageCounter;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            _inGameView = (InGameUIView)View;
            _inGameModel = (InGameUIModel)Model;
            
            _sabotageList = new List<Sabotage>();
            
            _inGameModel.SetEventChannel(openUIChannel);
            
            AddListener();
            SceneManager.sceneLoaded += SceneLoadedHandler;
            SceneManager.sceneUnloaded += SceneUnLoadedHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            RemoveListener();
            SceneManager.sceneLoaded -= SceneLoadedHandler;
            SceneManager.sceneUnloaded -= SceneUnLoadedHandler;
        }

        private void SceneUnLoadedHandler(Scene _)
        {
            _sabotageList.Clear();
            _inGameView.OnViewChange(false);
        }

        private void SceneLoadedHandler(Scene scene, LoadSceneMode __)
        {
            _sceneSabotageCounter = 0;
            _inGameModel.InitializeData(new InGameUIData
                ((int)playerStat.Hp, playerStat.ViewMapCooldown, playerStat.RunCooldown, playerStat.IsGun));
            _inGameView.UpdateView();
            if (scene.buildIndex >= stageInfoSO.tutorialIdx 
                && scene.buildIndex <= stageInfoSO.stageStartIdx + stageInfoSO.stageCount - 1)
                _inGameView.OpenView();
            else
                _inGameView.CloseView();
        }
        
        private void AddListener()
        {
            cameraChannel.AddListener<TopViewEvent>(UseTopViewSkillHandler);
            cameraChannel.AddListener<RegisterSabotageEvent>(AddSabotage);
            playerChannel.AddListener<SprintEndEvent>(SprintEndEventHandler);
            cameraChannel.AddListener<FirstViewComplete>(OnFirstViewEventHandler);
            playerChannel.AddListener<HitEvent>(HitEventEventHandler);
            playerChannel.AddListener<BulletChangeEvent>(BulletChangeEventHandler);
            playerChannel.AddListener<WeaponChangeEvent>(WeaponChangeEventHandler);
            playerChannel.AddListener<BulletShortageEvent>(BulletShortageEventHandler);
        }

        private void BulletShortageEventHandler(BulletShortageEvent data)
        {
            _inGameView.BulletWarning();
        }

        private void RemoveListener()
        {
            cameraChannel.RemoveListener<TopViewEvent>(UseTopViewSkillHandler);
            cameraChannel.RemoveListener<RegisterSabotageEvent>(AddSabotage);
            playerChannel.RemoveListener<SprintEndEvent>(SprintEndEventHandler);
            cameraChannel.RemoveListener<FirstViewComplete>(OnFirstViewEventHandler);
            playerChannel.RemoveListener<HitEvent>(HitEventEventHandler);
            playerChannel.RemoveListener<BulletChangeEvent>(BulletChangeEventHandler);
            playerChannel.RemoveListener<WeaponChangeEvent>(WeaponChangeEventHandler);
            playerChannel.RemoveListener<BulletShortageEvent>(BulletShortageEventHandler);
        }

        private void AddSabotage(RegisterSabotageEvent data)
        {
            if (data.Register == false)
                return;
            
            _sabotageList.Add(data.Sabotage);
            if (_inGameView.GetRedMarkCount > _sceneSabotageCounter++)
                return;
            _inGameView.AddRedMark(Instantiate(redMarkPrefab).GetComponent<RectTransform>());
        }

        private void UseTopViewSkillHandler(TopViewEvent data)
        {
            _inGameView.OnViewChange(data.IsTopView);
            if (data.IsTopView == false)
                return;

            Camera cam = Camera.main;
            List<Vector2> redMarkScreenPosList = new List<Vector2>();
            for (int i = 0; i < _sabotageList.Count; ++i)
            {
                Vector2 boxSize = _sabotageList[i].markBoxSize;
                if (_sabotageList[i].ShouldMark == true)
                {
                    redMarkScreenPosList.Add(cam.WorldToScreenPoint(
                        new Vector3(boxSize.x, 0, boxSize.y) + _sabotageList[i].transform.position));
                }
            }
            _inGameView.SetRedMark(redMarkScreenPosList);
        }

        private void SprintEndEventHandler(SprintEndEvent data)
        {
            _inGameModel.SetSprintSkillCooldown();
            _inGameView.UpdateForm((int)InGameUIEnum.SprintCover);
        }
        
        private void OnFirstViewEventHandler(FirstViewComplete data)
        {
            if (data.IsFirstViewComplete == false)
                return;
            
            _inGameModel.SetTopViewSkillCooldown();
            _inGameView.UpdateForm((int)InGameUIEnum.TopViewCover);
        }

        private void HitEventEventHandler(HitEvent data)
        {
            _inGameModel.SetCurrentHp((int)data.Hp);
            _inGameView.UpdateForm((int)InGameUIEnum.HpGauge);
            _inGameView.UpdateForm((int)InGameUIEnum.HpText);
        }

        private void BulletChangeEventHandler(BulletChangeEvent data)
        {
            _inGameModel.SetCurrentBullet(data.Bullet);
            _inGameView.UpdateForm((int)InGameUIEnum.BulletText);
        }

        private void WeaponChangeEventHandler(WeaponChangeEvent data)
        {
            _inGameModel.SetCurrentWeapon(data.IsGun
                ? (int)InGameUIData.WeaponType.Gun
                : (int)InGameUIData.WeaponType.Sword);
            _inGameView.UpdateForm((int)InGameUIEnum.WeaponField);
        }
    }
}