using System;
using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Multiable;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.Utility;
using UnityEngine;

namespace HwanLib.MVP.System.AbstractMVP
{
    public abstract class AbstractPopupView : BaseView, IMultiableView
    {
        public bool CanOpen => !RootCanvas.gameObject.activeSelf;
        public float OpenDuration { get; set; } = 0.25f;
        public float CloseDuration { get; set; } = 0.225f;

        public event Action OnClosed;

        protected DoTweenWindowForm WindowForm;
        private BackgroundForm _backgroundForm;
        
        private CanvasGroup _canvasGroup;
        public bool IsOpen { get; protected set; }

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
            
            WindowForm.OnAnimationEnd += AnimationEndHandler;

            IsOpen = false;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            WindowForm.OnAnimationEnd -= AnimationEndHandler;
        }

        public override void OpenView()
        {
            if (IsOpen == true)
                return;

            base.OpenView();
            
            IsOpen = true;
            WindowForm.PlayAnimation(true, OpenDuration);
            _backgroundForm?.DoFade(true, OpenDuration);
            
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
        public override void CloseView()
        {
            if (IsOpen == false)
                return;
            
            IsOpen = false;
            WindowForm.PlayAnimation(false, CloseDuration);
            _backgroundForm?.DoFade(false, CloseDuration);
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        protected void AnimationEndHandler()
        {
            if (IsOpen == false)
            {
                RootCanvas.gameObject.SetActive(false);
                OnClosed?.Invoke();
            }
        }
    }
}