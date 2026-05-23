using System;
using System.Collections.Generic;
using _Script.SaveSystem;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private EventChannelSO openUIEvent;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private FormSoundModule<SettingUIEnum> soundModule;
        [SerializeField] private StageInfoSO stageInfo;
        [SerializeField] private PlayerInputSO inputSO;
        
        private SettingUIView _settingView;
        private SettingUIModel _settingModel;
        private bool _prevLock;
        private float _prevTimeScale;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
                        
            _settingView = (SettingUIView)View;
            _settingModel = (SettingUIModel)Model;

            _settingModel.AudioMixer = audioMixer;
            _settingModel.StageInfo = stageInfo;
            _settingModel.OpenUIChannel = openUIEvent;
            _settingView.IsInGame = SceneManager.GetActiveScene().buildIndex != stageInfo.titleIdx;
            
            openUIEvent.AddListener<OpenSettingEvent>(ShowSetting);
            inputSO.OnOpenSettingUI += OpenSetting;
            _settingView.OnClosed += CloseViewHandler;
        }

        private void OpenSetting() => openUIEvent.RaiseEvent(OpenUIEvents.OpenSettingEvent);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            openUIEvent.RemoveListener<OpenSettingEvent>(ShowSetting);
            inputSO.OnOpenSettingUI -= OpenSetting;
            _settingView.OnClosed -= CloseViewHandler;
        }
        
        private void ShowSetting(OpenSettingEvent eventData)
        {
            if (_settingView.CanOpen == false)
            {
                _settingView.CloseView();
                return;
            }
            
            _settingView.OpenView();
            
            _prevTimeScale = Time.timeScale;
            Time.timeScale = 0;

            _prevLock = !Cursor.visible;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void CloseViewHandler()
        {
            saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
            
            Time.timeScale = _prevTimeScale;

            Cursor.visible = !_prevLock;
            Cursor.lockState = _prevLock ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
        protected override void InteractedHandler(int childIndex, UIParam value)
        {
            base.InteractedHandler(childIndex, value);
            soundModule.PlaySound(childIndex);
        }
    }
}