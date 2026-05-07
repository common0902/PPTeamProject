using System.Diagnostics;
using System.Linq;
using System.Reflection;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UIElements;

namespace _Works._CJW.Scripts.Objects.Sabotage.Editor
{
    [CustomEditor(typeof(AbstractSabotage), true)]
    public class SabotageEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visual = default;
        private static string _targetEvent;
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            visual.CloneTree(root);
            
            DropdownField eventsField = root.Q<DropdownField>("EventsDropdownField");
            RefreshEvents(eventsField);
            eventsField.RegisterCallback<ChangeEvent<string>>(HandleChange);
            eventsField.value = _targetEvent;
            return root;
        }

        private void HandleChange(ChangeEvent<string> evt)
        {
            AbstractSabotage sabotage = (AbstractSabotage)target;
            if(evt.previousValue == evt.newValue) return;
            sabotage.targetEventName = evt.newValue;
            _targetEvent = evt.newValue;
            EditorUtility.SetDirty(sabotage);
            AssetDatabase.SaveAssets();
        }

        private void RefreshEvents(DropdownField eventsField)
        {
            eventsField.choices.Clear();
            FieldInfo[] fields = typeof(SabotageEvents)
                .GetFields(BindingFlags.Public | BindingFlags.Static);
            eventsField.choices.AddRange(fields.Select(field => field.Name));
            eventsField.value = _targetEvent;
        }
    }
}