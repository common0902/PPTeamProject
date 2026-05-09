using HwanLib.MVP.Forms.Module.Cooldown;
using HwanLib.MVP.Forms.Module.Gauge;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class CooldownForm : AbstractVisualForm
    {
        private GaugeType _gaugeType;
        private AbstractCooldown _cooldown;
        private Coroutine _cooldownCoroutine;

        public void InitCooldownForm(GaugeType gaugeType)
        {
            _gaugeType = gaugeType;
            
            switch (_gaugeType)
            {
                case GaugeType.PosY:
                    _cooldown = new PosYCooldown();
                    break;
            }
            
            _cooldown.InitGauge(gameObject);
        }

        protected override void UpdateVisual(UIParam data)
        {
            UICooldownParam cooldownData = (UICooldownParam)data;
            
            if (_cooldown.GaugeRatio != 0)
                StopCoroutine(_cooldownCoroutine);
            
            _cooldown.SetCoolTime(cooldownData.Cooldown);
            _cooldown.GaugeRatio = cooldownData.Ratio;
            if (cooldownData.Ratio != 0)
                _cooldownCoroutine = StartCoroutine(_cooldown.StartCooldown());
        }
    }
}