using System;
using System.Runtime.InteropServices.ComTypes;
using DG.Tweening;
using HwanLib.MVP.System.AbstractMVP.Form;
using HwanLib.MVP.System.BaseMVP;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class BackgroundForm : AbstractClickForm, IInitializable
    {
        [Header("Default Fade Alpha")]
        [SerializeField] private float fadeInAlpha = 0.6f;
        
        [Header("Default Duration")]
        [SerializeField] private float fadeInDuration = 0.3f;
        [SerializeField] private float fadeOutDuration = 0.3f;

        public event Action<bool> OnFadeEnd;
        
        private Image _backgroundImage;
        private CanvasGroup _canvasGroup;
        
        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            
            GenerateBackground();
        }
        
        private void GenerateBackground()
        {
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            
            var image = GetComponent<Image>();
            image.color = new Color(0, 0, 0, fadeInAlpha);;

            // stretch, stretch 설정
            transform.localScale = Vector3.one;
            RectTransform rectTrm = GetComponent<RectTransform>();
            rectTrm.anchorMin = Vector2.zero;
            rectTrm.anchorMax = Vector2.one;
            rectTrm.offsetMax = Vector2.zero;
            rectTrm.offsetMin = Vector2.zero;
            
            transform.SetParent(rootCanvas.transform, false);
            transform.SetAsFirstSibling();

            _backgroundImage = image;
        }
        
        public void DoFade(bool isFadeIn, float duration, float targetAlpha)
        {
            _backgroundImage.DOComplete();
            _backgroundImage.DOKill();
            
            _backgroundImage.color = new Color(0, 0, 0, targetAlpha);
            _canvasGroup.DOFade(isFadeIn ? 1 : 0, duration).SetUpdate(true).SetEase(Ease.Linear)
                .OnComplete(() => OnFadeEnd?.Invoke(isFadeIn));
        }
        
        public void DoFade(bool isFadeIn)
            => DoFade(isFadeIn, isFadeIn ? fadeInDuration : fadeOutDuration, fadeInAlpha);
                
        public void DoFade(bool isFadeIn, float duration)
            => DoFade(isFadeIn, duration, fadeInAlpha);
    }
}