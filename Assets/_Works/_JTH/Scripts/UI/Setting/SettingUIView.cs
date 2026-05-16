using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.System.MVPModule;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIView : AbstractPopupView
    {
        private DoTweenWindowForm _windowForm;
        private BackgroundForm _backgroundForm;
        private bool _isOpen;
        private CanvasGroup _canvasGroup;

        protected override int WindowFormIndex => (int)SettingUIEnum.PopupWindow;
        protected override int BackgroundFormIndex => (int)SettingUIEnum.Background;
        protected override bool UseBackgroundForm => true;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            AddFormInteractionListener(CloseView, (int)SettingUIEnum.CloseBtn);
            AddFormInteractionListener(CloseView, (int)SettingUIEnum.Background);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            RemoveFormInteractionListener(CloseView, (int)SettingUIEnum.CloseBtn);
            RemoveFormInteractionListener(CloseView, (int)SettingUIEnum.Background);
        }
    }
}