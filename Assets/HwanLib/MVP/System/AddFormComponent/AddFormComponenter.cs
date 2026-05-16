using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace HwanLib.MVP.System.AddFormComponent
{
    public static class AddFormComponenter
    {
        public static BaseForm AddFormComponent(this GameObject gameObject, string formName)
        {
            if (gameObject.TryGetComponent(out BaseForm form))
                return form;
            
            switch (formName)
            {
                case "AccessForm":
                    return gameObject.AddComponent<AccessForm>();
                case "BackgroundForm":
                    return gameObject.AddComponent<BackgroundForm>();
                case "DoTweenWindowForm":
                    return gameObject.AddComponent<DoTweenWindowForm>();
                case "FadeButtonForm":
                    return gameObject.AddComponent<FadeButtonForm>();
                case "SliderForm":
                    return gameObject.AddComponent<SliderForm>();
                case "SwapForm":
                    return gameObject.AddComponent<SwapForm>();
                case "LockButtonForm":
                    return gameObject.AddComponent<LockButtonForm>();
                case "TextForm":
                    return gameObject.AddComponent<TextForm>();
                case "ToggleForm":
                    return gameObject.AddComponent<ToggleForm>();
                case "GaugeForm":
                    return gameObject.AddComponent<GaugeForm>();
                case "CooldownForm":
                    return gameObject.AddComponent<CooldownForm>();
                default:
                    return null;
            }
        }
    }
}
