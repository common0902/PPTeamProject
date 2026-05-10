using System;
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
        private AbstractGauge _cooldown;
        private Coroutine _cooldownCoroutine;

        public void InitCooldownForm(GaugeType gaugeType)
        {
            _gaugeType = gaugeType;
            
            switch (_gaugeType)
            {
                case GaugeType.PosY:
                    _cooldown = new PosYGauge();
                    break;
            }
            
            _cooldown.InitGauge(gameObject);
        }

        protected override void UpdateVisual(UIParam data)
        {
            UICooldownParam cooldownData = (UICooldownParam)data;
            _cooldown.SetGauge(cooldownData.Ratio);
            _cooldown.SetGauge(0, cooldownData.Cooldown);
        }

        private void OnDestroy()
        {
            _cooldown.OnDestroy();
        }
    }
}