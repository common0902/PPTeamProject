using UnityEngine;

namespace HwanLib.MVP.Forms.Module.Gauge
{
    internal class PosYGauge : AbstractGauge
    {
        private Transform _targetTransform;

        protected override void Initialize(GameObject gameObject)
        {
            _targetTransform = gameObject.transform;
        }

        protected override void SetGauge()
            => _targetTransform.localScale = new Vector3(_targetTransform.localScale.x,
                GaugeRatio, _targetTransform.localScale.z);
    }
}