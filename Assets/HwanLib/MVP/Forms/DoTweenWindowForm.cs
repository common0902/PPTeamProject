using System;
using DG.Tweening;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class DoTweenWindowForm : BaseForm
    {
        [field: SerializeField] public float OpenDuration { get; private set; } = 0.25f;
        [field: SerializeField] public float CloseDuration { get; private set; } = 0.225f;
        
        public event Action OnAnimationEnd;

        private Sequence _sequence;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
        }

        public void PlayOpenAnimation()
        {
            if (_sequence.IsActive() == true)
            {
                _sequence.Complete();
                _sequence.Kill();
                OnAnimationEnd?.Invoke();
            }
            transform.localScale = Vector3.zero;

            _sequence = DOTween.Sequence();
            float curDuration = Mathf.Clamp01(1 - transform.localScale.x) * OpenDuration;
            _sequence.Append(transform.DOScale(Vector3.one, curDuration).SetEase(Ease.InCirc))
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.one;
                    OnAnimationEnd?.Invoke();
                });
        }
        
        public void PlayCloseAnimation()
        {
            if (_sequence.IsActive() == true)
            {
                _sequence.Complete();
                _sequence.Kill();
                OnAnimationEnd?.Invoke();
            }
            transform.localScale = Vector3.one;

            _sequence = DOTween.Sequence();
            float curDuration = transform.localScale.x * CloseDuration;
            _sequence.Append(transform.DOScale(Vector3.zero, curDuration).SetEase(Ease.InBack))
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.zero;
                    OnAnimationEnd?.Invoke();
                });
        }

        private void OnDestroy()
        {
            if (_sequence.IsActive() == true)
            {
                _sequence.Complete();
                _sequence.Kill();
            }
        }
    }
}