using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;
using TMPro;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextForm : BaseForm
    {
        private TextMeshProUGUI _text;

        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);
            
            _text = GetComponent<TextMeshProUGUI>();
        }

        protected override void SetVisual(ChangedData changedData)
        {
            base.SetVisual(changedData);
            _text.text = ((UIStringParam)changedData).Value;
        }
    }
}