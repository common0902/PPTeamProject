using System;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using TMPro;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextForm : AbstractVisualForm, IUpdatable
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

        protected override void UpdateVisual(UIParam data)
        {
            string text = ((UIStringParam)data)?.Value;
            
            if (!String.IsNullOrEmpty(text))
                Text = String.Format(Text, text);
        }
    }
}