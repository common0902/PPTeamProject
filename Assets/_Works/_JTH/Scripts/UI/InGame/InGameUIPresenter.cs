using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.Sabotage;
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
        [SerializeField] private int stageStartIndex;
        [SerializeField] private int stageEndIndex;
        [SerializeField] private PlayerStatSO playerStat;
        
        private InGameUIModel _inGameModel;
        private InGameUIView _inGameView;

        private List<Sabotage> _sabotageList; 

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            _inGameView = (InGameUIView)View;
            _inGameModel = (InGameUIModel)Model;
            
            _inGameModel.SetEventChannel(openUIChannel);
            _inGameModel.InitializeData(new InGameUIData
                ((int)playerStat.Hp, playerStat.ViewMapCooldown, playerStat.RunCooldown, playerStat.IsGun));
            _inGameModel.InitializeData(new InGameUIData
                (1, 1, 1, true));

            AddListener();
            
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex >= stageStartIndex && currentIndex <= stageEndIndex )
                _inGameView.OpenView();
        }

        private void AddListener()
        {
            cameraChannel.AddListener<TopViewEvent>(UseTopViewSkillHandler);
            cameraChannel.AddListener<RegisterSabotageEvent>(AddSabotage);
            playerChannel.AddListener<SprintEndEvent>(SprintEndEventHandler);
            playerChannel.AddListener<HitEvent>(HitEventEventHandler);
            playerChannel.AddListener<BulletChangeEvent>(BulletChangeEventHandler);
            playerChannel.AddListener<WeaponChangeEvent>(WeaponChangeEventHandler);
        }

        private void RemoveListener()
        {
            cameraChannel.RemoveListener<TopViewEvent>(UseTopViewSkillHandler);
            cameraChannel.RemoveListener<RegisterSabotageEvent>(AddSabotage);
            playerChannel.RemoveListener<SprintEndEvent>(SprintEndEventHandler);
            playerChannel.RemoveListener<HitEvent>(HitEventEventHandler);
            playerChannel.RemoveListener<BulletChangeEvent>(BulletChangeEventHandler);
            playerChannel.RemoveListener<WeaponChangeEvent>(WeaponChangeEventHandler);
        }

        private void AddSabotage(RegisterSabotageEvent data)
        {
            if (data.Register == false)
                return; 
            _inGameView.AddRedMark(Instantiate(redMarkPrefab).GetComponent<RectTransform>());
            _sabotageList.Add(data.Sabotage);
        }

        protected override void OnDestroy()
        {
            RemoveListener();
            base.OnDestroy();
        }

        private void UseTopViewSkillHandler(TopViewEvent data)
        {
            _inGameView.OnViewChange(data.IsTopView);
            if (data.IsTopView == false)
                return;

            Func<Vector3, Vector3> getScreenPos = Camera.main.WorldToScreenPoint;
            List<Vector2> redMarkScreenPosList = new List<Vector2>();
            for (int i = 0; i < _sabotageList.Count; ++i)
            {
                Vector2 boxSize = _sabotageList[i].markBoxSize;
                if (_sabotageList[i].ShouldMark == true)
                    redMarkScreenPosList.Add(getScreenPos(
                        new Vector3(boxSize.x, 0, boxSize.y) + _sabotageList[i].transform.position));
            }
            _inGameView.SetRedMark(redMarkScreenPosList);
        }

        private void SprintEndEventHandler(SprintEndEvent data)
            => _inGameModel.SetSprintSkillCooldown();
        
        private void HitEventEventHandler(HitEvent data)
            => _inGameModel.SetCurrentHp((int)data.Hp);
                
        private void BulletChangeEventHandler(BulletChangeEvent data)
            => _inGameModel.SetCurrentBullet(data.Bullet);
                
        private void WeaponChangeEventHandler(WeaponChangeEvent data)
            => _inGameModel.SetCurrentWeapon(data.IsGun 
                ? (int)InGameUIData.WeaponType.Gun : (int)InGameUIData.WeaponType.Sword);
    }
}