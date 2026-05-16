using System;
using DG.Tweening;
using HwanLib.MVP.System.AbstractMVP.Form;
using HwanLib.MVP.System.BaseMVP;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(Image))]
    public class BackgroundForm : AbstractClickForm, IInitializable
    {
        [Header("Default Color")]
        [SerializeField] private Color fadeInColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        
        [Header("Default Duration")]
        [SerializeField] private float fadeInDuration = 0.3f;
        [SerializeField] private float fadeOutDuration = 0.3f;

        public event Action<bool> OnFadeEnd;
        
        private Image _backgroundImage;
        
        public void Initialize()
        {
            GenerateBackground();
        }
        
        private void GenerateBackground()
        {
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            
            var image = GetComponent<Image>();
            image.color = Color.clear;

            transform.localScale = Vector3.one;
            GetComponent<RectTransform>().sizeDelta = rootCanvas.GetComponent<RectTransform>().sizeDelta;
            transform.SetParent(rootCanvas.transform, false);
            transform.SetAsFirstSibling();

            _backgroundImage = image;
        }

        public void DoFade(float duration, Color targetColor)
        {
            _backgroundImage.DOComplete();
            _backgroundImage.DOKill();
            
            _backgroundImage.color = targetColor == Color.clear ? _backgroundImage.color : Color.clear;
            _backgroundImage.DOColor(targetColor, duration).SetUpdate(true).SetEase(Ease.Linear)
                .OnComplete(() => OnFadeEnd?.Invoke(targetColor != Color.clear));
        }
        
        public void DoFade(bool isFadeIn)
            => DoFade(isFadeIn ? fadeInDuration : fadeOutDuration, isFadeIn ? fadeInColor : Color.clear);
                
        public void DoFade(bool isFadeIn, float duration)
            => DoFade(duration, isFadeIn ? fadeInColor : Color.clear);
    }
}