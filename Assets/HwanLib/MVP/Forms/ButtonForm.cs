using HwanLib.MVP.System;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class ButtonForm : BaseForm
    {
        private Button _button;

        public override void InitializeForm(InteractObject onInteractObject)
        {
            base.InitializeForm(onInteractObject);
            
            _button = GetPartOfFormComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            OnInteractive<ChangedData>(null);
        }
    }
}
