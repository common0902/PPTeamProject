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
        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            GetForm<GaugeForm>((int)InGameUIEnum.HpGauge).InitGaugeForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.ShiftCover).InitCooldownForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.TabCover).InitCooldownForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.QCover).InitCooldownForm(GaugeType.PosY);
        }
    }
}