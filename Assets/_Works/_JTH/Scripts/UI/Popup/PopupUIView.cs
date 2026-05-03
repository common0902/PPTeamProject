using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.UI;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIView : BaseView
    {
        private readonly Color _backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        
        private Image _backgroundImage;
        private DoTweenWindowForm _windowForm;
        private bool _isOpen;
        private CanvasGroup _canvasGroup;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _windowForm = GetForm<DoTweenWindowForm>((int)PopupUIEnum.RootWindow);
            _canvasGroup = RootCanvas.GetComponent<CanvasGroup>();
            
            AddFormInteractionListener(StartCloseAnimation, (int)PopupUIEnum.YesBtn);
            AddFormInteractionListener(StartCloseAnimation, (int)PopupUIEnum.NoBtn);

            _windowForm.OnAnimationEnd += AnimationEndHandler;

            GenerateBackground();
            _isOpen = false;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            RemoveFormInteractionListener(StartCloseAnimation, (int)PopupUIEnum.YesBtn);
            RemoveFormInteractionListener(StartCloseAnimation, (int)PopupUIEnum.NoBtn);
            _windowForm.OnAnimationEnd -= AnimationEndHandler;
        }

        public override void OpenView()
        {
            base.OpenView();
            
            SetBackgroundActive(true);
            
            _windowForm.transform.localScale = Vector3.zero;
            _windowForm.PlayOpenAnimation();
            
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void StartCloseAnimation()
        {
            SetBackgroundActive(false);

            _windowForm.PlayCloseAnimation();
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        
        private void AnimationEndHandler()
        {
            _isOpen = !_isOpen;

            if (_isOpen == false)
            {
                RootCanvas.gameObject.SetActive(false);
            }
        }

        private void GenerateBackground()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, _backgroundColor);
            bgTex.Apply();

            GameObject background = new GameObject("Background");
            var image = background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;

            background.transform.localScale = new Vector3(1, 1, 1);
            background.GetComponent<RectTransform>().sizeDelta = RootCanvas.GetComponent<RectTransform>().sizeDelta;
            background.transform.SetParent(RootCanvas.transform, false);
            background.transform.SetAsFirstSibling();

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