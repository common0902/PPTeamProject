using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AbstractMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Setting
{
    public class SettingUIView : AbstractPopupView
    {
        protected override int WindowFormIndex => (int)SettingUIEnum.PopupWindow;
        protected override int BackgroundFormIndex => (int)SettingUIEnum.Background;
        protected override bool UseBackgroundForm => true;

        public bool IsInGame { get; set; }

        private AccessForm _inGameBtns;
        private ToggleForm _toggle;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            AddFormInteractionListener(CloseView, (int)SettingUIEnum.CloseBtn);
            AddFormInteractionListener(CloseView, (int)SettingUIEnum.Background);

            _inGameBtns = GetForm<AccessForm>((int)SettingUIEnum.InGameBtns);
            _toggle = GetForm<ToggleForm>((int)SettingUIEnum.FullScreenToggle);
        }

        public override void OpenView()
        {
            base.OpenView();
            
            _inGameBtns.gameObject.SetActive(IsInGame);
            _toggle.gameObject.SetActive(!IsInGame);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            RemoveFormInteractionListener(CloseView, (int)SettingUIEnum.CloseBtn);
            RemoveFormInteractionListener(CloseView, (int)SettingUIEnum.Background);
        }
    }
}