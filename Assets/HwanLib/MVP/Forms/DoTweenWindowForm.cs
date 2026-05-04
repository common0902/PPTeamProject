using System;
using DG.Tweening;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class DoTweenWindowForm : BaseForm
    {
        [SerializeField] private float openDuration = 0.25f;
        [SerializeField] private float closeDuration = 0.225f;
        
        public event Action OnAnimationEnd;

        private Sequence _sequence;

        public void PlayOpenAnimation()
        {
            transform.localScale = Vector3.zero;
            if (_sequence != null && _sequence.IsActive() == true)
            {
                _sequence.Kill();
                OnAnimationEnd?.Invoke();
            }

            _sequence = DOTween.Sequence();
            float curDuration = Mathf.Clamp01(1 - transform.localScale.x) * openDuration;
            _sequence.Append(transform.DOScale(Vector3.one, curDuration).SetEase(Ease.InCirc))
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.one;
                    OnAnimationEnd?.Invoke();
                });
        }
        
        public void PlayCloseAnimation()
        {
            transform.localScale = Vector3.one;
            if (_sequence != null && _sequence.IsActive() == true)
            {
                _sequence.Kill();
                OnAnimationEnd?.Invoke();
            }

            _sequence = DOTween.Sequence();
            float curDuration = transform.localScale.x * closeDuration;
            _sequence.Append(transform.DOScale(Vector3.zero, curDuration).SetEase(Ease.InBack))
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.zero;
                    OnAnimationEnd?.Invoke();
                });
        }

        private void OnDestroy()
        {
            if (_sequence != null)
            {
                _sequence.Complete();
                _sequence.Kill();
            }
        }
    }
}