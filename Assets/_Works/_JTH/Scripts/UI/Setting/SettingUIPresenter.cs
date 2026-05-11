using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.System.SaveMVP;
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

            _settingModel.SetAudioMixer(audioMixer);
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

#if UNITY_EDITOR
        private void Update()
        {
            if (!Keyboard.current.ctrlKey.isPressed)
                return;
            
            if (Keyboard.current.sKey.wasPressedThisFrame)
                TestOpen();
        }
        
        public void TestOpen()
        {
            ShowSetting(OpenUIEvents.OpenSettingEvent);
        }
#endif
    }
}