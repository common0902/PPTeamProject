using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using _Works._JTH.Scripts.UI.Event;
using _Works._JYG._Script;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIModel : ISaveableModel
    {
        private class SettingInfo
        {
            public float MasterVolume = 0.5f;
            public float BgmVolume = 0.5f;
            public float SfxVolume = 0.5f;
            public bool IsFullScreen = true;
        }
        
        public AudioMixer AudioMixer { get; set; }
        public StageInfoSO StageInfo { get; set; }
        public EventChannelSO OpenUIChannel { get; set; }

        private SettingInfo _settingInfo;

        public void SetDefaultValue()
        {
            _settingInfo ??= new SettingInfo();
        }

        public string StoreData()
        {
            return JsonUtility.ToJson(_settingInfo);
        }

        public void RestoreData(string data)
        {
            _settingInfo = JsonUtility.FromJson<SettingInfo>(data);
            AudioMixer.SetFloat("Master", Mathf.Log10(_settingInfo.MasterVolume) * 20f);
            AudioMixer.SetFloat("BGM", Mathf.Log10(_settingInfo.BgmVolume) * 20f);
            AudioMixer.SetFloat("SFX", Mathf.Log10(_settingInfo.SfxVolume) * 20f);
            
            if (_settingInfo.IsFullScreen)
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            else
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }

        private void MasterVolumeChangeHandler(UIParam data)
        {
            float volume = ((UIFloatParam)data).Value;
            _settingInfo.MasterVolume = volume;
            AudioMixer.SetFloat("Master", Mathf.Log10(_settingInfo.MasterVolume) * 20f);
        }
        
        private void BGMVolumeChangeHandler(UIParam data)
        {
            float volume = ((UIFloatParam)data).Value;
            _settingInfo.BgmVolume = volume;
            AudioMixer.SetFloat("BGM", Mathf.Log10(_settingInfo.BgmVolume) * 20f);
        }
        
        private void SFXVolumeChangeHandler(UIParam data)
        {
            float volume = ((UIFloatParam)data).Value;
            _settingInfo.SfxVolume = volume;
            AudioMixer.SetFloat("SFX", Mathf.Log10(_settingInfo.SfxVolume) * 20f);
        }

        private void FullScreenToggleChangeHandler(UIParam data)
        {
            bool isFullScreen = ((UIBoolParam)data).Value;
            _settingInfo.IsFullScreen = isFullScreen;
            
            if (_settingInfo.IsFullScreen)
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            else
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }

        private void TitleBtnClickHandler(UIParam data)
        {
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent.Init(StageInfo.titleIdx, false, false));
        }
        
        private void RestartBtnClickHandler(UIParam data)
        {
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(SceneManager.GetActiveScene().buildIndex, false, false));
            GameManager.Instance.Player.GetComponent<CapsuleCollider>().enabled = false;
        }
        
        private UIParam UpdateMasterVolume() => UIParams.UIFloatParam.Init(_settingInfo.MasterVolume);
        
        private UIParam UpdateBGMVolume() => UIParams.UIFloatParam.Init(_settingInfo.BgmVolume);
        
        private UIParam UpdateSFXVolume() => UIParams.UIFloatParam.Init(_settingInfo.SfxVolume);
        
        private UIParam UpdateIsFullScreen() => UIParams.UIBoolParam.Init(_settingInfo.IsFullScreen);
    }
}