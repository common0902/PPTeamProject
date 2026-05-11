using DG.Tweening;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace HwanLib.MVP.Forms.Module.Gauge
{
    internal class PosYGauge : AbstractGauge
    {
        private RectTransform _targetTransform;

        protected override void Init(GameObject gameObject)
        {
            _targetTransform = gameObject.GetComponent<RectTransform>();
            _targetTransform.pivot = new Vector2(0.5f, 0);
        }

        public override void SetGauge(float ratio, float duration = 0, Ease ease = Ease.Linear)
        {
            _targetTransform.DOKill(true);
            _targetTransform.DOScaleY(ratio, duration).SetEase(ease);
        }

        public override void OnDestroy()
        {
            _targetTransform.DOKill();
        }

        public override void StopCooldown()
        {
            _targetTransform.DOPause();
        }

        public override void StartCooldown()
        {
            _targetTransform.DOPlay();
        }
    }
}