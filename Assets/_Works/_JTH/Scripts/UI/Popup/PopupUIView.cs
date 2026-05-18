using System.Collections.Generic;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AbstractMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIView : AbstractPopupView
    {
        protected override int WindowFormIndex => (int)PopupUIEnum.Popup;
        protected override int BackgroundFormIndex => (int)PopupUIEnum.Background;
        protected override bool UseBackgroundForm => true;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            AddFormInteractionListener(CloseView, (int)PopupUIEnum.YesBtn);
            AddFormInteractionListener(CloseView, (int)PopupUIEnum.NoBtn);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            RemoveFormInteractionListener(CloseView, (int)PopupUIEnum.YesBtn);
            RemoveFormInteractionListener(CloseView, (int)PopupUIEnum.NoBtn);
        }
    }
}