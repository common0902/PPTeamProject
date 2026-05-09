using UnityEngine;

namespace HwanLib.MVP.Forms.Module.Cooldown
{
    internal class PosYCooldown : AbstractCooldown
    {
        private Transform _targetTransform;

        protected override void Initialize(GameObject gameObject)
        {
            _targetTransform = gameObject.transform;
        }

        protected override void SetGauge()
            => _targetTransform.localScale = new Vector3(_targetTransform.localScale.x,
                _targetTransform.localScale.y * GaugeRatio, _targetTransform.localScale.z);
    }
}