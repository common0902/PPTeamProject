using System;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class BackgroundForm : AbstractClickForm
    {
        private class TempComponent : MonoBehaviour
        {
            public Action OnActiveTrue;
            public Action OnActiveFalse;

            private void OnEnable()
                => OnActiveTrue?.Invoke();

            private void OnDisable()
                => OnActiveFalse?.Invoke();
        }
        
        private readonly Color _backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        
        private Image _backgroundImage;
        private TempComponent _tempCompo;
        
        private void Awake()
        {
            GenerateTempGameObject();
            GenerateBackground();
        }

        private void GenerateTempGameObject()
        {
            GameObject tempGameObject = new GameObject();
            tempGameObject.name = "TempGameObject"; 
            
            tempGameObject.transform.SetParent(transform.parent, false);
            tempGameObject.transform.SetAsFirstSibling();
            _tempCompo = tempGameObject.AddComponent<TempComponent>();
            
            _tempCompo.OnActiveTrue += ActiveTrueHandler;
            _tempCompo.OnActiveFalse += ActiveFalseHandler;
        }

        private void OnDestroy()
        {
            _tempCompo.OnActiveTrue -= ActiveTrueHandler;
            _tempCompo.OnActiveFalse -= ActiveFalseHandler;
        }

        private void ActiveTrueHandler()
            => SetBackgroundActive(true);

        private void ActiveFalseHandler()
            => SetBackgroundActive(false);

        private void GenerateBackground()
        {
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            
            var image = gameObject.AddComponent<Image>();
            image.color = _backgroundColor;

            gameObject.transform.localScale = Vector3.one;
            gameObject.GetComponent<RectTransform>().sizeDelta = rootCanvas.GetComponent<RectTransform>().sizeDelta;
            gameObject.transform.SetParent(rootCanvas.transform, false);
            gameObject.transform.SetAsFirstSibling();

            _backgroundImage = image;
        }

        private void SetBackgroundActive(bool active)
        {
            if (active == true)
            {
                _backgroundImage.canvasRenderer.SetAlpha(0.0f);
                _backgroundImage.CrossFadeAlpha(1.0f, 0.2f, false);
            }
            else
            {
                _backgroundImage.canvasRenderer.SetAlpha(1.0f);
                _backgroundImage.CrossFadeAlpha(0.0f,0.2f, false);
            }
        }
    }
}