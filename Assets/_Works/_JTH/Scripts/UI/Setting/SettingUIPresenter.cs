using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private EventChannelSO openUIEvent;
        [SerializeField] private AudioMixer audioMixer;
        
        private SettingUIView _settingView;
        private SettingUIModel _settingModel;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
                        
            _settingView = (SettingUIView)View;
            _settingModel = (SettingUIModel)Model;

            _settingModel.AudioMixer = audioMixer;
            openUIEvent?.AddListener<OpenSettingEvent>(ShowSetting);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            openUIEvent?.RemoveListener<OpenSettingEvent>(ShowSetting);
        }
        
        private void ShowSetting(OpenSettingEvent eventData)
        {
            _settingView.OpenView();
        }
    }
}