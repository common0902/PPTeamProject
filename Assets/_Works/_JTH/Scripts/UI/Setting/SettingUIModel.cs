using HwanLib.MVP.System;
using HwanLib.MVP.System.SaveMVP;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.Audio;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIModel : AbstractSaveableModel
    {
        private AudioMixer _audioMixer;

        private class SettingInfo
        {
            public float MasterVolume = 0.5f;
            public float BgmVolume = 0.5f;
            public float SfxVolume = 0.5f;
            public bool IsFullScreen = true;
        }
        
        private SettingInfo _settingInfo;

        public void SetAudioMixer(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }

        public override void SetDefaultData()
        {
            _settingInfo = new SettingInfo();
        }
        
        public override string StoreData()
        {
            return JsonUtility.ToJson(_settingInfo);
        }

        public override void RestoreData(string data)
        {
            _settingInfo = JsonUtility.FromJson<SettingInfo>(data);
            
            _audioMixer.SetFloat("Master", _settingInfo.MasterVolume);
            _audioMixer.SetFloat("BGM", _settingInfo.BgmVolume);
            _audioMixer.SetFloat("SFX", _settingInfo.SfxVolume);
            
            if (_settingInfo.IsFullScreen)
                Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
            else
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }

        private void MasterVolumeChangeHandler(UIParam data)
        {
            float volume = ((UIFloatParam)data).Value;
            _settingInfo.MasterVolume = volume;
            _audioMixer.SetFloat("Master", Mathf.Log10(_settingInfo.MasterVolume) * 20f);
        }
        
        private void BGMVolumeChangeHandler(UIParam data)
        {
            float volume = ((UIFloatParam)data).Value;
            _settingInfo.BgmVolume = volume;
            _audioMixer.SetFloat("BGM", Mathf.Log10(_settingInfo.BgmVolume) * 20f);
        }
        
        private void SFXVolumeChangeHandler(UIParam data)
        {
            float volume = ((UIFloatParam)data).Value;
            _settingInfo.SfxVolume = volume;
            _audioMixer.SetFloat("SFX", Mathf.Log10(_settingInfo.SfxVolume) * 20f);
        }

        private void FullScreenToggleChangeHandler(UIParam data)
        {
            bool isFullScreen = ((UIBoolParam)data).Value;
            _settingInfo.IsFullScreen = isFullScreen;
            
            if (_settingInfo.IsFullScreen)
                Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
            else
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }

        private void CloseBtnClickHandler(UIParam data)
        {
            
        }

        private UIParam UpdateMasterVolume() => UIParamData.UIFloatParam.Init(_settingInfo.MasterVolume);
        
        private UIParam UpdateBGMVolume() => UIParamData.UIFloatParam.Init(_settingInfo.BgmVolume);
        
        private UIParam UpdateSFXVolume() => UIParamData.UIFloatParam.Init(_settingInfo.SfxVolume);
        
        private UIParam UpdateIsFullScreen() => UIParamData.UIBoolParam.Init(_settingInfo.IsFullScreen);
    }
}