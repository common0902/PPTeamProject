using System;
using System.IO;
using HwanLib.MVP.Editor;
using HwanLib.MVP.System.AddFormComponent;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class AddFormComponentEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset editorView = default;

    private static string _generateFolderPath;
    
    private Button _generateBtn;
    private Button _generateFolderBtn;
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
        _generateFolderBtn = root.Q<Button>("GenerateBtn");
        _generateFolderLabel = root.Q<Label>("GenerateFolderLabel");
        _generateFolderLabel.text = "No folder selected";
        
        _generateFolderBtn.clicked += () => HandleFolderSelectBtn(ref _generateFolderPath, _generateFolderLabel);
        _generateBtn.clicked += HandleGenerateFormAddComponent;
        
        if (!string.IsNullOrEmpty(_generateFolderPath))
        {
            _generateFolderLabel.text = FileUtil.GetProjectRelativePath(_generateFolderPath);
        }
    }
    
    private void HandleGenerateFormAddComponent()
    {
        if (string.IsNullOrEmpty(_generateFolderPath) || !Directory.Exists(_generateFolderPath))
        {
            EditorUtility.DisplayDialog("Folder Not Found", "경로 설정이 올바르지 않습니다.", "OK");
            return;
        }

        string[] formNames = EditorInfo.FormNames;
        string formCases = String.Join('\n', formNames);
        
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
}
