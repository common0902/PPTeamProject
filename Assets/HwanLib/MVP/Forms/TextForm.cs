using System;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using TMPro;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextForm : BaseForm, IUpdatable
    {
        public string Text
        {
            get => _textMeshProUGUI.text;
            set => _textMeshProUGUI.text = value;
        }

        public event UpdateForm OnFormUpdate;

        private TextMeshProUGUI _textMeshProUGUI;

        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);
            
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }
        
        public void UpdateForm()
        {
            string data = ((UIStringParam)OnFormUpdate?.Invoke(ChildIndex))?.Value;
            
            if (!String.IsNullOrEmpty(data))
                Text = String.Format(Text, data);
        }
    }
}