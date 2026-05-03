using System;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class TextButtonForm : BaseForm
    {
        public string Text
        {
            get => _textForm.Text;
            set => _textForm.Text = value;
        }
        
        private TextForm _textForm;
        private ButtonForm _buttonForm;
        private string _savedText;
        private string _interactiveFalseText;

        public void SetTextAndButtonForm(TextForm textForm, ButtonForm buttonForm)
        {
            _textForm = textForm;
            _buttonForm = buttonForm;
        }
        
        public void SetInteractiveFalseText(string interactableFalseText)
            => _interactiveFalseText = interactableFalseText;

        public void SetInteractive(bool interactive)
        {
            Debug.Assert(!String.IsNullOrEmpty(_interactiveFalseText), "_interactiveFalseText is empty");
            
            if (interactive == true && _buttonForm.Interactive == false)
            {
                _textForm.Text = _savedText;
            }
            else if (interactive == false && _buttonForm.Interactive == true)
            {
                _savedText = _textForm.Text;
                _textForm.Text = _interactiveFalseText;
            }
            
            _buttonForm.SetInteractive(interactive);
        }
    }
}
