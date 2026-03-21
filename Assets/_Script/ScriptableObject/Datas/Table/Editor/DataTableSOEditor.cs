using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Script.ScriptableObject.Datas.Table.Editor
{
    [UnityEditor.CustomEditor(typeof(AbstractDataTableSO), true)]
    public class DataTableSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default; 
        public override UnityEngine.UIElements.VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            editorView.CloneTree(root);

            root.Q<Button>("GenerateButton").clicked += HandleGenerateButtonClick;
            
            
            return root;
        }

        private void HandleGenerateButtonClick()
        {
            AbstractDataTableSO dataTable = target as AbstractDataTableSO;

            int index = 0;
            foreach (IndexSO asset in dataTable.AssetList)
            {
                asset.Index = index++;
                EditorUtility.SetDirty(asset);
            }
            
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}