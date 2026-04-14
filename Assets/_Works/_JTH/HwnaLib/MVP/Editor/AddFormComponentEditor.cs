using System;
using System.IO;
using HwnaLib.MVP.System.AddFormComponent;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class AddFormComponentEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset editorView = default;

    private string[] _forms;
    private static string _formFolderPath;
    private static string _generateFolderPath;
    private static AssemblyDefinitionAsset _formAssembly;
    
    private Button _generateBtn;
    private Button _formFolderBtn;
    private Button _generateFolderBtn;
    private Label _formFolderLabel;
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
        
        _generateBtn = root.Q<Button>("FormFolderBtn");
        _formFolderBtn = root.Q<Button>("GenerateFolderBtn");
        _generateFolderBtn = root.Q<Button>("GenerateBtn");
        _formFolderLabel = root.Q<Label>("FormFolderLabel");
        _generateFolderLabel = root.Q<Label>("GenerateFolderLabel");
        _formFolderLabel.text = "No folder selected";
        _generateFolderLabel.text = "No folder selected";
        
        _formFolderBtn.clicked += () => HandleFolderSelectBtn(ref _formFolderPath, _formFolderLabel);
        _generateFolderBtn.clicked += () => HandleFolderSelectBtn(ref _generateFolderPath, _generateFolderLabel);
        _generateBtn.clicked += HandleGenerateFormAddComponent;
        
        if (!string.IsNullOrEmpty(_formFolderPath))
        {
            _formFolderLabel.text = FileUtil.GetProjectRelativePath(_formFolderPath);
        }
        if (!string.IsNullOrEmpty(_generateFolderPath))
        {
            _generateFolderLabel.text = FileUtil.GetProjectRelativePath(_generateFolderPath);
        }
    }
    
    private void HandleGenerateFormAddComponent()
    {
        if (_forms == null || _forms.Length == 0)
        {
            EditorUtility.DisplayDialog("Error"
                , "Form이 정의된 파일이 필요합니다.", "OK");
        }
        if (string.IsNullOrEmpty(_generateFolderPath) || !Directory.Exists(_generateFolderPath))
        {
            EditorUtility.DisplayDialog("Folder Not Found", "경로 설정이 올바르지 않습니다.", "OK");
            return;
        }

        string formCases = String.Join('\n', _forms);
        
        string code = string.Format(CodeFormat.ComponenterFormat, formCases);
        File.WriteAllText($"{_generateFolderPath}/FormAddComponenter.cs", code);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void HandleFolderSelectBtn(ref string folderPath, Label folderLabel)
    {
        folderPath = EditorUtility.OpenFolderPanel("폴더를 선택", folderPath, "");

        if (!string.IsNullOrEmpty(folderPath))
        {
            folderLabel.text = FileUtil.GetProjectRelativePath(folderPath);
        }
    }

    private void FillForms()
    {
        if (string.IsNullOrEmpty(_formFolderPath) || !Directory.Exists(_formFolderPath))
        {
            EditorUtility.DisplayDialog("Folder Not Found", "경로 설정이 올바르지 않습니다.", "OK");
            return;
        }
    }
}
