using DG.Tweening;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.System.MVPModule.Form;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(Image))]
    public class BackgroundForm : AbstractClickForm
    {
        private readonly Color _fadeInColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        private readonly Color _fadeOutColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0);
        
        private Image _backgroundImage;
        private float _fadeInDuration;
        private float _fadeOutDuration;
        
        private void Awake()
        {
            GenerateBackground();
            _fadeInDuration = 0;
            _fadeOutDuration = 0;
        }

        public void SetDuration(float duration)
        { 
            _fadeInDuration = duration;
            _fadeOutDuration = duration;
        } 

        public void SetDuration(float fadeInDuration, float fadeOutDuration)
        {
            _fadeInDuration = fadeInDuration;
            _fadeOutDuration = fadeOutDuration;
        }
        
        private void GenerateBackground()
        {
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            
            var image = gameObject.GetComponent<Image>();
            image.color = _fadeOutColor;

            gameObject.transform.localScale = Vector3.one;
            gameObject.GetComponent<RectTransform>().sizeDelta = rootCanvas.GetComponent<RectTransform>().sizeDelta;
            gameObject.transform.SetParent(rootCanvas.transform, false);
            gameObject.transform.SetAsFirstSibling();

            _backgroundImage = image;
        }

        public void DoFade(bool fadeIn)
        {
            _backgroundImage.DOKill();
            
            if (fadeIn == true)
            {
                _backgroundImage.color = _fadeOutColor;
                _backgroundImage.DOColor(_fadeInColor, _fadeInDuration).SetUpdate(true);
            }
            else
            {
                _backgroundImage.color = _fadeInColor;
                _backgroundImage.DOColor(_fadeOutColor, _fadeInDuration).SetUpdate(true);
            }
        }
    }
}