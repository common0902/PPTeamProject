namespace HwanLib.MVP.System.AddFormComponent
{
    public static class CodeFormat
    {
        public static string AddFormComponenterFormat =
@"using HwanLib.MVP.Forms;
using UnityEngine;

namespace {0}
{{
    public static class FormAddComponenter
    {{
        public static BaseForm AddFormComponent(this GameObject gameObject, string formName)
        {{
            switch (formName)
            {{{1}
                default:
                    return null;
            }}
        }}
    }}
}}
";

        public static string CaseFormat = 
@"
                case ""{0}"":
                    return gameObject.AddComponent<{0}>();";
    }

}