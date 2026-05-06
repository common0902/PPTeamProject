using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class ToggleForm : AbstractVisualForm, IInteractable
    {
        public event FormInteracted OnFormInteracted;

        private Toggle _toggle;

        public void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(SliderValueChangeHandler);
        }

        private void SliderValueChangeHandler(bool value)
        {
            OnFormInteracted?.Invoke(ChildIndex, UIParamData.UIBoolParam.Init(value));
        }

        protected override void UpdateVisual(UIParam data)
        {
            _toggle.isOn = ((UIBoolParam)data).Value;
        }
    }
}