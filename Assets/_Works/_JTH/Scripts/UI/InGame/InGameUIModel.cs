using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIModel : IModel
    {
        private EventChannelSO _openUIChannel;
        private InGameUIData _inGameData;

        public void SetEventChannel(EventChannelSO openUIChannel)
            => _openUIChannel = openUIChannel;

        private UIParam UpdateHpText()
            => UIParamContainer.UIStringParam.Init(_inGameData.CurrentHp.ToString());

        private UIParam UpdateBulletText()
            => UIParamContainer.UIStringParam.Init(_inGameData.RemainingBullets.ToString());

        private UIParam UpdateQSkillCover()
            => UIParamContainer.UICooldownParam.Init(_inGameData.RemainingQSkillCooldown, 
                _inGameData.RemainingQSkillCooldown / _inGameData.MaxQSkillCooldown);

        private UIParam UpdateTabSkillCover()
            => UIParamContainer.UICooldownParam.Init(_inGameData.RemainingTabSkillCooldown, 
                _inGameData.RemainingTabSkillCooldown / _inGameData.MaxTabSkillCooldown);

        private UIParam UpdateShiftSkillCover()
            => UIParamContainer.UICooldownParam.Init(_inGameData.RemainingShiftSkillCooldown, 
                _inGameData.RemainingShiftSkillCooldown / _inGameData.MaxShiftSkillCooldown);

        private UIParam UpdateHpGauge()
            => UIParamContainer.UIFloatParam.Init((float)_inGameData.CurrentHp / _inGameData.MaxHp);

        private UIParam UpdateWeaponSwap()
            => UIParamContainer.UISwapParam.Init((int)_inGameData.CurrentWeapon, 0);

        private void SettingBtnClickHandler(UIParam clickData)
            => _openUIChannel.RaiseEvent(OpenUIEvents.OpenSettingEvent);

        public void InitializeData(InGameUIData data)
            => _inGameData = data;
        
        public void SetCurrentWeapon(int weapon)
            => _inGameData.CurrentWeapon = (InGameUIData.WeaponType)weapon;
        
        public void SetCurrentHp(int hp)
            => _inGameData.CurrentHp = hp;
        
        public void SetQSkillCooldown(float cooldown)
            => _inGameData.RemainingQSkillCooldown = cooldown;
        
        public void SetQSkillCooldown()
            => _inGameData.RemainingQSkillCooldown = _inGameData.MaxQSkillCooldown;
                
        public void SetTabSkillCooldown(float cooldown)
            => _inGameData.RemainingTabSkillCooldown = cooldown;
        
        public void SetTabSkillCooldown()
            => _inGameData.RemainingTabSkillCooldown = _inGameData.MaxTabSkillCooldown;
        
        public void SetShiftSkillCooldown(float cooldown)
            => _inGameData.RemainingShiftSkillCooldown = cooldown;
        
        public void SetShiftSkillCooldown()
            => _inGameData.RemainingShiftSkillCooldown = _inGameData.MaxShiftSkillCooldown;
    }
}