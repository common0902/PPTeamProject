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
                case "TextButtonForm":
                    return gameObject.AddComponent<TextButtonForm>();
                default:
                    return null;
            }
        }
    }
}
