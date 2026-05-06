using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.System.SaveMVP;
using UnityEngine;
using UnityEngine.Audio;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private EventChannelSO openUIEvent;
        [SerializeField] private AudioMixer audioMixer;
        
        private SettingUIView _settingView;
        private SettingUIModel _settingModel;

        public override void InitializePresenter(MVPDataSO dataSO)
        {
            base.InitializePresenter(dataSO);
            
            _settingView = View as SettingUIView;
            _settingModel = Model as SettingUIModel;
            
            _settingModel.SetAudioMixer(audioMixer);
            openUIEvent?.AddListener<OpenSettingEvent>(ShowSetting);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            openUIEvent?.RemoveListener<OpenSettingEvent>(ShowSetting);
        }

#if UNITY_EDITOR
        [ContextMenu("TestOpen")]
        public void TestOpen()
        {
            ShowSetting(OpenUIEvents.OpenSettingEvent);
        }
#endif
        
        private void ShowSetting(OpenSettingEvent eventData)
        {
            _settingView.OpenView();
        }
    }
}