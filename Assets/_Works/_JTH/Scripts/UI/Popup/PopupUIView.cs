using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIView : BaseView
    {
        private DoTweenWindowForm _windowForm;
        private bool _isOpen;
        private CanvasGroup _canvasGroup;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _windowForm = GetForm<DoTweenWindowForm>((int)PopupUIEnum.Popup);
            _canvasGroup = RootCanvas.GetComponent<CanvasGroup>();
            
            AddFormInteractionListener(StartCloseAnimation, (int)PopupUIEnum.YesBtn);
            AddFormInteractionListener(StartCloseAnimation, (int)PopupUIEnum.NoBtn);

            _windowForm.OnAnimationEnd += AnimationEndHandler;

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
            
            _windowForm.PlayOpenAnimation();
            
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void StartCloseAnimation()
        {
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
    }
}