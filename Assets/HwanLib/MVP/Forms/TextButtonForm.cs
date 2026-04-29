using HwanLib.MVP.System;
using HwanLib.MVP.UIData;
using TMPro;

namespace HwanLib.MVP.Forms
{
    public class TextButtonForm : ButtonForm
    {
        private TextMeshProUGUI _tmpPro;

        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);
            
            _tmpPro = GetPartOfFormComponent<TextMeshProUGUI>();
        }
        
        protected override void SetVisual(ChangedData changedData)
        {
            base.SetVisual(changedData);
            
            _tmpPro.text = (changedData as UIStringParam)?.Value;
        }
    }
}
