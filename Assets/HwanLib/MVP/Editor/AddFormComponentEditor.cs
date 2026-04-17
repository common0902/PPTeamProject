using System.Linq;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AddFormComponent;
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

            GenerateScriptEditor generator = new(root, "AddFormComponenter"
                , "AddFormComponenterPath", GenerateCode);
        }
    
        private string GenerateCode(string folderPath, string scriptName)
        {
            string[] formNames = EditorInfo.GetTypeNames(typeof(BaseForm), false).ToArray();
            string formCase = "";
            
            foreach (var formName in formNames)
                formCase += string.Format(CodeFormat.CaseFormat , formName);
        
            string nameSpace = FileUtil.GetProjectRelativePath(folderPath).Substring("Assets/".Length);
            nameSpace = nameSpace.Replace("/", ".");
        
            return string.Format(CodeFormat.AddFormComponenterFormat, nameSpace, scriptName, formCase);
        }
    }
}
