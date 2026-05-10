using UnityEngine;

namespace HwanLib.MVP.Forms.Module.Gauge
{
    internal abstract class AbstractGauge
    {
        private float _gaugeRatio;
        
        public virtual float GaugeRatio
        {
            get => Mathf.Clamp01(_gaugeRatio);
            set
            {
                _gaugeRatio = Mathf.Clamp01(value);
                SetGauge();
            }
        }

        public void InitGauge(GameObject gameObject)
        {
            Initialize(gameObject);
            GaugeRatio = 0;
        }

        protected abstract void Initialize(GameObject gameObject);

        protected abstract void SetGauge();
    }
}