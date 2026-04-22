using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP;
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
                case "ButtonForm":
                    return gameObject.AddComponent<ButtonForm>();
                case "DoTweenWindowForm":
                    return gameObject.AddComponent<DoTweenWindowForm>();
                case "TextButtonForm":
                    return gameObject.AddComponent<TextButtonForm>();
                case "TextForm":
                    return gameObject.AddComponent<TextForm>();
                default:
                    Debug.LogErrorFormat("Unknown form name: {0}", formName);
                    return null;
            }
        }
    }
}
