using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.Utility;
using UnityEngine;

namespace HwanLib.MVP.System.MVPModule
{
    public abstract class AbstractPopupView : BaseView
    {
        public bool CanUse => !RootCanvas.gameObject.activeSelf;

        protected DoTweenWindowForm WindowForm;
        private BackgroundForm _backgroundForm;
        
        private CanvasGroup _canvasGroup;
        private bool _isOpen;

        protected abstract int WindowFormIndex { get; }
        protected abstract int BackgroundFormIndex { get; }
        protected abstract bool UseBackgroundForm { get; }

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            WindowForm = GetForm<DoTweenWindowForm>(WindowFormIndex);
            _backgroundForm = UseBackgroundForm ? GetForm<BackgroundForm>(BackgroundFormIndex) : null;
            _canvasGroup = RootCanvas.gameObject.GetOrAddComponent<CanvasGroup>();
            
            _backgroundForm?.SetDuration(WindowForm.OpenDuration, WindowForm.CloseDuration);
            
            WindowForm.OnAnimationEnd += AnimationEndHandler;

            _isOpen = false;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            WindowForm.OnAnimationEnd -= AnimationEndHandler;
        }

        public override void OpenView()
        {
            if (_isOpen == true)
                return;

            base.OpenView();
            
            _isOpen = true;
            WindowForm.PlayOpenAnimation();
            _backgroundForm?.DoFade(true);
            
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
        public override void CloseView()
        {
            if (_isOpen == false)
                return;
            
            _isOpen = false;
            WindowForm.PlayCloseAnimation();
            _backgroundForm?.DoFade(false);
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        protected void AnimationEndHandler()
        {
            if (_isOpen == false)
                RootCanvas.gameObject.SetActive(false);
        }
    }
}