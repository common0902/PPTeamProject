using HwanLib.MVP.Forms;
using UnityEngine;

namespace HwanLib.MVP.System.AddFormComponent
{
    public static class FormAddComponenter
    {
        public static BaseForm AddFormComponent(this GameObject gameObject, string formName)
        {
            switch (formName)
            {
                case "ButtonForm":
                    return gameObject.AddComponent<ButtonForm>();
                case "DFJKFDForm":
                    return gameObject.AddComponent<DFJKFDForm>();
                case "ToggleForm":
                    return gameObject.AddComponent<ToggleForm>();
                default:
                    return null;
            }
        }
    }
}
