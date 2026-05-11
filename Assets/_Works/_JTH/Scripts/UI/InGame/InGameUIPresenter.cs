using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.Sabotage;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIPresenter : BasePresenter
    {
        [SerializeField] private GameObject redMarkPrefab;
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private EventChannelSO cameraChannel;
        [SerializeField] private int stageStartIndex;
        [SerializeField] private int stageEndIndex;
        
        private InGameUIModel _inGameModel;
        private InGameUIView _inGameView;

        private List<Sabotage> _sabotageList; 

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            _inGameView = (InGameUIView)View;
            _inGameModel = (InGameUIModel)Model;
            
            _inGameModel.SetEventChannel(openUIChannel);
            _inGameModel.InitializeData(new InGameUIData(100, 2.5f, 2.5f, 3f));
            
            cameraChannel.AddListener<TopViewEvent>(UseTopViewSkillHandler);
            cameraChannel.AddListener<RegisterSabotageEvent>(AddSabotage);
            
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex >= stageStartIndex && currentIndex <= stageEndIndex )
                _inGameView.OpenView();
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
            cameraChannel.AddListener<TopViewEvent>(UseTopViewSkillHandler);
            
            base.OnDestroy();
        }

        private void UseTopViewSkillHandler(TopViewEvent data)
        {
            _inGameView.OnViewChange(data.IsTopView);
            if (data.IsTopView == false)
                return;
            _inGameView.SetRedMark(new Vector2[3]);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Keyboard.current.ctrlKey.isPressed)
                return;

            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                _inGameView.OnViewChange(true);
                Vector2[] a = new Vector2[3];
                for (int i = 0; i < 3; ++i)
                {
                    a[i] = Random.insideUnitCircle * 3;
                    Debug.Log(a[i]);
                }
                _inGameView.SetRedMark(a);
            }
            
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                _inGameView.OnViewChange(false);
            }
        }
        #endif
    }
}