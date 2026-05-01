using System;
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
        public string Text
        {
            get => _textMeshProUGUI.text;
            set => _textMeshProUGUI.text = value;
        }

        private TextMeshProUGUI _textMeshProUGUI;

        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);
            
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        protected override void SetVisual(ChangedData changedData)
        {
            base.SetVisual(changedData);
            Text = String.Format(Text, ((UIStringParam)changedData).Value);
        }
    }
}