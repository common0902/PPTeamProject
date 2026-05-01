using System.Linq;
using System.Reflection;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AddFormComponent;
using HwanLib.MVP.System.BaseMVP;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HwanLib.MVP.Editor
{
    public class AddFormComponentEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset editorView = default;

        [MenuItem("Tools/Generate AddFormComponent")]
        public static void ShowExample()
        {
            AddFormComponentEditor wnd = GetWindow<AddFormComponentEditor>();
            wnd.titleContent = new GUIContent("AddFormComponentEditor");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
        
            editorView.CloneTree(root);

            _ = new GenerateScriptEditor(root, "AddFormComponenter"
                , "AddFormComponenterPath", GenerateCode);
        }
    
        private string GenerateCode(string folderPath, string scriptName)
        {
            //MVP System의 Assembly에서 찾기
            string[] formNames = Assembly.GetAssembly(typeof(BasePresenter)).GetTypes()
                .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract 
                               && type.IsSubclassOf(typeof(BaseForm)))
                .Select(type => type.Name).ToArray();
            string formCase = "";
            
            foreach (var formName in formNames)
                formCase += string.Format(CodeFormat.CaseFormat , formName);
        
            string nameSpace = FileUtil.GetProjectRelativePath(folderPath).Substring("Assets/".Length);
            nameSpace = nameSpace.Replace("/", ".");
        
            return string.Format(CodeFormat.AddFormComponenterFormat, nameSpace, scriptName, formCase);
        }
    }
}
