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
                case "FadeButtonForm":
                    return gameObject.AddComponent<FadeButtonForm>();
                case "DoTweenWindowForm":
                    return gameObject.AddComponent<DoTweenWindowForm>();
                case "SliderForm":
                    return gameObject.AddComponent<SliderForm>();
                case "TextButtonForm":
                    return gameObject.AddComponent<TextButtonForm>();
                case "TextForm":
                    return gameObject.AddComponent<TextForm>();
                case "ToggleForm":
                    return gameObject.AddComponent<ToggleForm>();
                default:
                    return null;
            }
        }
    }
}
