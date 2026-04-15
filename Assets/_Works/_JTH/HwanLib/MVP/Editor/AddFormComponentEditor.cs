using System.IO;
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

        private string _folderPath;
    
        private Button _generateBtn;
        private Button _folderSelectBtn;
        private Label _generateFolderLabel;

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
        
            _generateBtn = root.Q<Button>("GenerateBtn");
            _folderSelectBtn = root.Q<Button>("FolderSelectBtn");
            _generateFolderLabel = root.Q<Label>("GenerateFolderLabel");
            _generateFolderLabel.text = "No folder selected";
        
            _folderSelectBtn.clicked += () => HandleFolderSelectBtn(ref _folderPath, _generateFolderLabel);
            _generateBtn.clicked += HandleGenerateFormAddComponent;
        
            if (!string.IsNullOrEmpty(_folderPath))
            {
                _generateFolderLabel.text = FileUtil.GetProjectRelativePath(_folderPath);
            }

            string folderPath = PlayerPrefs.GetString("AddFormComponenterPath");
            if (!string.IsNullOrEmpty(folderPath))
            {
                _folderPath = folderPath;
                _generateFolderLabel.text = FileUtil.GetProjectRelativePath(_folderPath);
            }
        }
    
        private void HandleGenerateFormAddComponent()
        {
            if (string.IsNullOrEmpty(_folderPath) || !Directory.Exists(_folderPath))
            {
                EditorUtility.DisplayDialog("Folder Not Found", "경로 설정이 올바르지 않습니다.", "OK");
                return;
            }

            string[] formNames = EditorInfo.GetTypeNames(typeof(BaseForm), false).ToArray();
            string formCase = "";
            foreach (var formName in formNames)
                formCase += string.Format(CodeFormat.CaseFormat , formName);
        
            string nameSpace = FileUtil.GetProjectRelativePath(_folderPath).Substring("Assets/".Length); 
            if (nameSpace.StartsWith("_Works/_JTH/"))
            {
                nameSpace = nameSpace.Substring("_Works/_JTH/".Length);
            }

            nameSpace = nameSpace.Replace("/", ".");
        
            string code = string.Format(CodeFormat.AddFormComponenterFormat, nameSpace, formCase);
            File.WriteAllText($"{_folderPath}/FormAddComponenter.cs", code);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void HandleFolderSelectBtn(ref string folderPath, Label folderLabel)
        {
            folderPath = EditorUtility.OpenFolderPanel("폴더를 선택", folderPath, "");

            if (!string.IsNullOrEmpty(folderPath))
            {
                folderLabel.text = FileUtil.GetProjectRelativePath(folderPath);
                PlayerPrefs.SetString("AddFormComponenterPath", folderPath);
            }
        }
    }
}
