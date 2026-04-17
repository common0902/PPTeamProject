using HwanLib.MVP.System;
using HwanLib.MVP.UIData;
using TMPro;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class TextButtonForm : BaseForm
    {
        private TextMeshProUGUI _tmpPro;
        private Button _button;

        public override void InitializeForm(InteractObject onInteractObject)
        {
            base.InitializeForm(onInteractObject);
            
            _tmpPro = GetPartOfFormComponent<TextMeshProUGUI>();
            _button = GetPartOfFormComponent<Button>();
            
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            OnInteractive(UIDataContainer.StringChangedData);
        }
        
        protected override void SetVisual<TUseData>(TUseData data)
        {
            _tmpPro.text = (data as StringChangedData)?.Value;
        }
    }
}
