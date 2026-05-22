using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AbstractMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.InteractiveBtn
{
    public class InteractiveBtnUIView : AbstractPopupView
    {
        protected override int WindowFormIndex => (int)InteractiveBtnUIEnum.Window;
        protected override int BackgroundFormIndex => -1;
        protected override bool UseBackgroundForm => false;
        
        private RectTransform _window;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _window = GetForm<DoTweenWindowForm>((int)InteractiveBtnUIEnum.Window)
                .GetComponent<RectTransform>();

            _window.anchorMin = Vector2.one / 2;
            _window.anchorMax = Vector2.one / 2;
        }

        public void MoveToTargetTransform(Camera mainCam, Transform targetTransform)
        {
            if (targetTransform == null)
                return;
            
            Vector2 pos = mainCam.WorldToScreenPoint(targetTransform.position);
            _window.anchoredPosition = pos;
        }
    }
}