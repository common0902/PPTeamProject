using System;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class TextButtonForm : BaseForm
    {
        public string Text => _textForm.Text;
        
        private TextForm _textForm;
        private FadeButtonForm _fadeButtonForm;
        private string _interactiveFalseText;

        public void SetTextAndButtonForm(TextForm textForm, FadeButtonForm fadeButtonForm)
        {
            _textForm = textForm;
            _fadeButtonForm = fadeButtonForm;

            _fadeButtonForm.OnFormInteracted += UpdateText;
        }

        private void OnDestroy()
        {
            _fadeButtonForm.OnFormInteracted -= UpdateText;
        }

        private void UpdateText(int _, UIParam __) => _textForm.UpdateForm();
        
        public void SetInteractiveFalseText(string interactableFalseText)
            => _interactiveFalseText = interactableFalseText;

        public void SetInteractive(bool interactive)
        {
            Debug.Assert(!String.IsNullOrEmpty(_interactiveFalseText), "_interactiveFalseText is empty");
            
            if (interactive == false)
            {
                _textForm.Text = _interactiveFalseText;
            }
            
            _fadeButtonForm.SetInteractive(interactive);
        }
    }
}
