using HwanLib.MVP.Forms.Module.Gauge;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;

namespace HwanLib.MVP.Forms
{
    public class GaugeForm : AbstractVisualForm
    {
        private GaugeType _gaugeType;
        private AbstractGauge _gauge;

        public void InitGaugeForm(GaugeType gaugeType)
        {
            _gaugeType = gaugeType;
            
            switch (_gaugeType)
            {
                case GaugeType.PosY:
                    _gauge = new PosYGauge();
                    break;
            }
            
            _gauge.InitGauge(gameObject);
        }

        protected override void UpdateVisual(UIParam data)
        {
            float ratio = ((UIFloatParam)data).Value;
            
            _gauge.GaugeRatio = ratio;
        }
    }
}