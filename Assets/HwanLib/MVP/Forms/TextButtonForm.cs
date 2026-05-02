using System;
using HwanLib.MVP.System;
using HwanLib.MVP.UIData;
using TMPro;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextButtonForm : ButtonForm
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        
        private string _originText;
        private string _savedText;
        private string _interactiveFalseText;

        public string Text
        {
            get => textMeshProUGUI.text;
            private set => textMeshProUGUI.text = value;
        }

        public void InitTextButtonForm(string interactableFalseText)
            => _interactiveFalseText = interactableFalseText;
        
        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);

            if (textMeshProUGUI == null)
            {
                textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            }
            
            _originText = textMeshProUGUI.text;
        }

        protected override void SetVisual(ChangedData changedData)
        {
            base.SetVisual(changedData);

            if (Interactive == true)
            {
                Text = String.Format(_originText, ((UIStringParam)changedData).Value);
            }
        }

        public override void SetInteractive(bool interactive)
        {
            Debug.Assert(!String.IsNullOrEmpty(_interactiveFalseText), "_interactiveFalseText is empty");
            
            if (interactive == true && Interactive == false)
            {
                Text = _savedText;
            }
            else if (interactive == false && Interactive == true)
            {
                _savedText = Text;
                Text = _interactiveFalseText;
            }
            
            base.SetInteractive(interactive);
        }
    }
}
