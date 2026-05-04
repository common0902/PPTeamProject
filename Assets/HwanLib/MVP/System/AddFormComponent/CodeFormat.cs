namespace HwanLib.MVP.System.AddFormComponent
{
    public static class CodeFormat
    {
        public static string AddFormComponenterFormat =
@"using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace {0}
{{
    public static class {1}
    {{
        public static BaseForm AddFormComponent(this GameObject gameObject, string formName)
        {{
            if (gameObject.TryGetComponent(out BaseForm form))
                return form;
            
            switch (formName)
            {{{2}
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
        
        public static string EnumFormat =
@"namespace {0}
{{
    public enum {1}
    {{
        {2}
    }}
}}
";
    }

}