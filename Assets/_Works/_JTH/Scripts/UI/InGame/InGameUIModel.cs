using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIModel : IModel
    {
        private EventChannelSO _openUIChannel;
        private InGameUIData _inGameData;

        public void SetEventChannel(EventChannelSO openUIChannel)
            => _openUIChannel = openUIChannel;

        private UIParam UpdateHpText()
            => UIParams.UIStringParam.Init($"{_inGameData.CurrentHp}/{_inGameData.MaxHp}");

        private UIParam UpdateBulletText()
            => UIParams.UIStringParam.Init($"{_inGameData.RemainingBullets : 00}");

        private UIParam UpdateTopViewSkillCover()
            => UIParams.UICooldownParam.Init(_inGameData.RemainingTabSkillCooldown, 
                _inGameData.RemainingTabSkillCooldown / _inGameData.MaxTopViewSkillCooldown);

        private UIParam UpdateSprintSkillCover()
            => UIParams.UICooldownParam.Init(_inGameData.RemainingSprintSkillCooldown, 
                _inGameData.RemainingSprintSkillCooldown / _inGameData.MaxSprintSkillCooldown);

        private UIParam UpdateHpGauge()
            => UIParams.UIFloatParam.Init((float)_inGameData.CurrentHp / _inGameData.MaxHp);

        private UIParam UpdateWeaponSwap()
            => UIParams.UISwapParam.Init((int)_inGameData.CurrentWeapon, 0);

        private void SettingBtnClickHandler(UIParam clickData)
            => _openUIChannel.RaiseEvent(OpenUIEvents.OpenSettingEvent);

        public void InitializeData(InGameUIData data)
            => _inGameData = data;
        
        public void SetCurrentWeapon(int weapon)
            => _inGameData.CurrentWeapon = (InGameUIData.WeaponType)weapon;
        
        public void SetCurrentHp(int hp)
            => _inGameData.CurrentHp = hp;
                
        public void SetTopViewSkillCooldown(float cooldown)
            => _inGameData.RemainingTabSkillCooldown = cooldown;
        
        public void SetTopViewSkillCooldown()
            => _inGameData.RemainingTabSkillCooldown = _inGameData.MaxTopViewSkillCooldown;
        
        public void SetSprintSkillCooldown(float cooldown)
            => _inGameData.RemainingSprintSkillCooldown = cooldown;
        
        public void SetSprintSkillCooldown()
            => _inGameData.RemainingSprintSkillCooldown = _inGameData.MaxSprintSkillCooldown;

        public void SetCurrentBullet(int dataBullet)
            => _inGameData.RemainingBullets = dataBullet;
    }
}