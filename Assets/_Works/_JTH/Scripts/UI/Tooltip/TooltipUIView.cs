using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Tooltip
{
    public class TooltipUIView : BaseView
    {
        private RectTransform _windowRectTrm;
        
        private DoTweenWindowForm _windowForm;
        private bool _isOpen;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _windowForm = GetForm<DoTweenWindowForm>((int)TooltipUIEnum.Window);
            _windowRectTrm = _windowForm.GetComponent<RectTransform>();
            
            _windowForm.OnAnimationEnd += AnimationEndHandler;

            _isOpen = false;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            _windowForm.OnAnimationEnd -= AnimationEndHandler;
        }

        public override void OpenView()
        {
            base.OpenView();
            
            _isOpen = true;
            _windowForm.PlayOpenAnimation();
        }

        public void CloseTooltip()
        {
            if (_isOpen == false)
                return;

            _isOpen = false;
            _windowForm.PlayCloseAnimation();
        }
        
        private void AnimationEndHandler()
        {
            if (_isOpen == false)
            {
                RootCanvas.gameObject.SetActive(false);
            }
        }

        public void OpenView(Vector2 tooltipPos)
        {
            if (_isOpen == true)
                return;
            
            OpenView();
            _windowRectTrm.anchoredPosition = tooltipPos;
        }
    }
}