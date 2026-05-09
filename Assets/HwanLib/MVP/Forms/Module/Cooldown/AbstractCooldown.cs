using System.Collections;
using HwanLib.MVP.Forms.Module.Gauge;
using UnityEngine;

namespace HwanLib.MVP.Forms.Module.Cooldown
{
    internal abstract class AbstractCooldown : AbstractGauge
    {
        private float _currentTime;
        private float _coolTime;

        public override float GaugeRatio
        {
            get
            {
                if (_coolTime <= 0)
                    return 0;
                return Mathf.Clamp01(_currentTime / _coolTime);
            }
            set
            {
                _currentTime = _coolTime * Mathf.Clamp01(value);
                SetGauge();
            }
        }

        public void SetCoolTime(float coolTime)
            => _coolTime = coolTime;

        public IEnumerator StartCooldown()
        {
            while (GaugeRatio != 0)
            {
                yield return null;
                _currentTime -= Time.deltaTime;
                SetGauge();
            }
            GaugeRatio = 0;
        }
    }
}