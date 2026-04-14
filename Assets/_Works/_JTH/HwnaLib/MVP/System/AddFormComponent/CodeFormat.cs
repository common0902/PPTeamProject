namespace HwnaLib.MVP.System.AddFormComponent
{
    public static class CodeFormat
    {
        public static string ComponenterFormat =@"
using HwanLib.MVP.System;
using UnityEngine;

namespace HwnaLib.MVP.System.AddFormComponent
{
    public static class FormAddComponenter
    {
        public static BaseForm AddComponent(this GameObject gameObject, string formName)
        {
            switch (formName)
            {
                {0}
            }
        }
    }
}
";

        public static string CaseFormat = @"
case ""{0}"":
    gameObject.AddComponent<{0}>();
    break;";
    }

}