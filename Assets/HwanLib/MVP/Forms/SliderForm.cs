using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class SliderForm : AbstractVisualForm, IInteractable
    {
        public event FormInteracted OnFormInteracted;

        private Slider _slider;

        public void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(SliderValueChangeHandler);
        }

        private void SliderValueChangeHandler(float value)
        {
            OnFormInteracted?.Invoke(ChildIndex, UIParamData.UIFloatParam.Init(value));
        }

        protected override void UpdateVisual(UIParam data)
        {
            _slider.value = ((UIFloatParam)data).Value;
        }
    }
}