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
            
            // 오른쪽에 있으면 길이 / 2만큼 왼쪽으로 이동, 반대면 반대로 이동
            // 위에 있으면 높이 / 2 만큼 아래로 이동, 반대면 위로 이동
            Vector2 offset = _windowRectTrm.sizeDelta / 2;
            Vector2 screenSize = Camera.main.ViewportToScreenPoint(Vector2.one);
            offset.x = tooltipPos.x >= screenSize.x / 2f ? -offset.x : offset.x;
            offset.y = tooltipPos.y >= screenSize.y / 2f ? -offset.y : offset.y;
            _windowRectTrm.anchoredPosition = tooltipPos + offset;
        }
    }
}