using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AbstractMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }

        public void MoveToTargetTransform(Vector2 movePos)
        {
            _window.anchoredPosition = movePos;
        }
    }
}