using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.Forms.Module.Gauge;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIView : BaseView
    {
        private SwapForm _weaponField;
        private AccessForm _sprintSkill;
        
        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);

            _weaponField = GetForm<SwapForm>((int)InGameUIEnum.WeaponField);
            _sprintSkill = GetForm<AccessForm>((int)InGameUIEnum.SprintSkill);
            
            GetForm<GaugeForm>((int)InGameUIEnum.HpGauge).InitGaugeForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.SprintCover).InitCooldownForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.TopViewCover).InitCooldownForm(GaugeType.PosY);
        }

        public void OnViewChange(bool isTopView)
        {
            if (isTopView == true)
            {
                _weaponField.enabled = false;
                _sprintSkill.enabled = false;
            }
            else
            {
                _weaponField.enabled = true;
                _sprintSkill.enabled = true;
            }
        }
    }
}