using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.System.MVPModule.Multiple;
using HwanLib.Utility;
using UnityEngine;

namespace HwanLib.MVP.System.MVPModule
{
    public abstract class AbstractPopupView : BaseView, IMultiple
    {
        public bool CanUse => !RootCanvas.gameObject.activeSelf;

        protected DoTweenWindowForm WindowForm;
        protected BackgroundForm BackgroundForm;
        
        protected CanvasGroup CanvasGroup;
        protected bool IsOpen;

        protected abstract int WindowFormIndex { get; }
        protected abstract int BackgroundFormIndex { get; }
        protected abstract bool UseBackgroundForm { get; }

        public override void InitializeView(Canvas rootCanvas, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(rootCanvas, formDataList, formInteractedHandler, updateFormHandler);
            
            WindowForm = GetForm<DoTweenWindowForm>(WindowFormIndex);
            BackgroundForm = UseBackgroundForm ? GetForm<BackgroundForm>(BackgroundFormIndex) : null;
            CanvasGroup = RootCanvas.gameObject.GetOrAddComponent<CanvasGroup>();
            
            BackgroundForm?.SetDuration(WindowForm.OpenDuration, WindowForm.CloseDuration);
            
            if (UseBackgroundForm) 
                AddFormInteractionListener(CloseView, BackgroundFormIndex);
            WindowForm.OnAnimationEnd += AnimationEndHandler;

            IsOpen = false;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            if (UseBackgroundForm) 
                RemoveFormInteractionListener(CloseView, BackgroundFormIndex);
            WindowForm.OnAnimationEnd -= AnimationEndHandler;
        }

        public override void OpenView()
        {
            if (IsOpen == true)
                return;

            base.OpenView();
            
            IsOpen = true;
            WindowForm.PlayOpenAnimation();
            BackgroundForm?.DoFade(true);
            
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }
        
        public override void CloseView()
        {
            if (IsOpen == false)
                return;
            
            IsOpen = false;
            WindowForm.PlayCloseAnimation();
            BackgroundForm?.DoFade(false);
            
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        protected void AnimationEndHandler()
        {
            if (IsOpen == false)
                RootCanvas.gameObject.SetActive(false);
        }
    }
}