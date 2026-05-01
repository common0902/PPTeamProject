using System.IO;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

namespace HwanLib.MVP.Editor
{
    public class GenerateScriptEditor
    {
        public delegate string GenerateCode(string folderPath, string scriptName);
        
        private string _folderPath;
        public string PrefsName { get; set; } 
        public string FileName { get; set; } 
        private readonly GenerateCode _codeGenerator;

        private Button _generateBtn;
        private Button _folderSelectBtn;
        private Label _folderLabel;
        
        public GenerateScriptEditor(VisualElement root, string fileName
            , string prefsName, GenerateCode codeGenerator)
        {
            InitializeUIElements(root);

            PrefsName = prefsName;
            _codeGenerator = codeGenerator;
            FileName = fileName;

            _folderPath = PlayerPrefs.GetString(PrefsName, null);
            if (!string.IsNullOrEmpty(_folderPath))
                _folderLabel.text = FileUtil.GetProjectRelativePath(_folderPath);
        }
        
        public GenerateScriptEditor(VisualElement root)
        {
            InitializeUIElements(root);
        }

        private void InitializeUIElements(VisualElement root)
        {
            _generateBtn = root.Q<Button>("GenerateBtn");
            _folderSelectBtn = root.Q<Button>("FolderSelectBtn");
            _folderLabel = root.Q<Label>("GenerateFolderLabel");

            _folderSelectBtn.clicked += HandleFolderSelect;
            _generateBtn.clicked += HandleGenerateScript;
        }

        private void HandleGenerateScript()
        {
            if (string.IsNullOrEmpty(_folderPath) || !Directory.Exists(_folderPath))
            {
                EditorUtility.DisplayDialog("Folder Not Found", "경로 설정이 올바르지 않습니다.", "OK");
                return;
            }
            
            File.WriteAllText($"{_folderPath}/{FileName}.cs", _codeGenerator?.Invoke(_folderPath, FileName));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void HandleFolderSelect()
        {
            _folderPath = EditorUtility.OpenFolderPanel("폴더를 선택", _folderPath, "");

            if (string.IsNullOrEmpty(_folderPath)) return;
            _folderLabel.text = FileUtil.GetProjectRelativePath(_folderPath);
                
            if (!string.IsNullOrEmpty(_folderPath))
            {
                PlayerPrefs.SetString(PrefsName, _folderPath);
            }
        }
    }
}