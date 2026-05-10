using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIPresenter : BasePresenter
    {
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private int stageStartIndex;
        [SerializeField] private int stageEndIndex;
        
        private InGameUIModel _inGameModel;
        private InGameUIView _inGameView;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            _inGameView = (InGameUIView)View;
            _inGameModel = (InGameUIModel)Model;
            
            _inGameModel.SetEventChannel(openUIChannel);
            _inGameModel.InitializeData(new InGameUIData(100, 2.5f, 2.5f, 3f));
            
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex >= stageStartIndex && currentIndex <= stageEndIndex )
                _inGameView.OpenView();
        }
        
        // #if UNITY_EDITOR
        //
        // private void Update()
        // {
        //     if (Keyboard.current.digit1Key.wasPressedThisFrame)
        //     {
        //         _inGameModel.SetCurrentWeapon((int)InGameUIData.WeaponType.Sword);
        //         _inGameView.UpdateForm((int)InGameUIEnum.WeaponField);
        //     }
        //     if (Keyboard.current.digit0Key.wasPressedThisFrame)
        //     {
        //         _inGameModel.SetCurrentWeapon((int)InGameUIData.WeaponType.Gun);
        //         _inGameView.UpdateForm((int)InGameUIEnum.WeaponField);
        //     }
        //
        //     if (Keyboard.current.qKey.wasPressedThisFrame)
        //     {
        //         _inGameModel.SetCurrentHp(50);
        //         _inGameView.UpdateForm((int)InGameUIEnum.HpText);
        //         _inGameView.UpdateForm((int)InGameUIEnum.HpGauge);
        //     }
        //
        //     if (Keyboard.current.eKey.wasPressedThisFrame)
        //     {
        //         _inGameModel.SetTopViewSkillCooldown();
        //         _inGameView.UpdateForm((int)InGameUIEnum.TopViewCover);
        //     }
        // }
        // #endif
    }
}