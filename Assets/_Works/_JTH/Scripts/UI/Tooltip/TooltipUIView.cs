using System;
using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Tooltip
{
    public class TooltipUIView : AbstractPopupView
    {
        private RectTransform _windowRectTrm;
        
        private AccessForm _lockIcon;
        private TextForm _desc;

        protected override int WindowFormIndex => (int)TooltipUIEnum.Window;
        protected override int BackgroundFormIndex => -1;
        protected override bool UseBackgroundForm => false;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _lockIcon = GetForm<AccessForm>((int)TooltipUIEnum.LockIcon);
            _desc = GetForm<TextForm>((int)TooltipUIEnum.Desc);
            
            _windowRectTrm = WindowForm.GetComponent<RectTransform>();
        }

        public override void OpenView()
        {
            base.OpenView();
            
            if (!String.IsNullOrEmpty(_desc.Text))
            { 
                _desc.gameObject.SetActive(true);
                _lockIcon.gameObject.SetActive(false);
            }
            else
            {
                _desc.gameObject.SetActive(false);
                _lockIcon.gameObject.SetActive(true);
            }
        }

        public void SetPosition(Vector2 tooltipPos)
        {
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