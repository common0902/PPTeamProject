using DG.Tweening;
using HwanLib.MVP.Forms.Module.Gauge;
using HwanLib.MVP.System.AbstractMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class GaugeForm : AbstractVisualForm
    {
        [SerializeField] private float duration = 0.2f;
        
        private GaugeType _gaugeType;
        private AbstractGaugeModule _gaugeModule;

        public void InitGaugeForm(GaugeType gaugeType)
        {
            _gaugeType = gaugeType;
            
            switch (_gaugeType)
            {
                case GaugeType.PosY:
                    _gaugeModule = new PosYGaugeModuleModule();
                    break;
            }
            
            _gaugeModule.InitGauge(gameObject);
        }

        protected override void UpdateVisual(UIParam data)
        {
            float ratio = ((UIFloatParam)data).Value;
            
            _gaugeModule.SetGauge(ratio, duration, Ease.OutQuart);
        }

        private void OnDestroy()
        {
            _gaugeModule.OnDestroy();
        }
    }
}