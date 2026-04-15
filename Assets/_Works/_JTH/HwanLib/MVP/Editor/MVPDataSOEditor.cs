using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace HwanLib.MVP.Editor
{
    [CustomEditor(typeof(MVPDataSO))]
    public class MVPDataSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default;

        private MVPDataSO _targetData;
        private ObjectField _uiPrefabObjectField;
        private DropdownField _modelTypeNameDropdown;
        private DropdownField _viewTypeNameDropdown;
        private DropdownField _childDropdown;
        private DropdownField _formTypeDropdown;
        private DropdownField _methodDropdown;
        private VisualElement _typeNameContainer;
        private VisualElement _formDataContainer;
        
        private FormData CurrentForm
        {
            get
            {
                if (string.IsNullOrEmpty(_targetData.selectedChildName))
                    return null;
                return _targetData.GetFormData(_targetData.selectedChildName);
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            _targetData = (MVPDataSO)target; 
            
            VisualElement root = new();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            editorView.CloneTree(root);

            _modelTypeNameDropdown = root.Q<DropdownField>("ModelTypeNameDropdown");
            _viewTypeNameDropdown = root.Q<DropdownField>("ViewTypeNameDropdown");
            _childDropdown = root.Q<DropdownField>("ChildObjectDropdown");
            _formTypeDropdown = root.Q<DropdownField>("FormTypeDropdown");
            _methodDropdown = root.Q<DropdownField>("TargetMethodNameDropdown");
            _uiPrefabObjectField = root.Q<ObjectField>("UIPrefabObjectField");
            _typeNameContainer = root.Q<VisualElement>("TypeNameContainer");
            _formDataContainer = root.Q<VisualElement>("FormDataContainer");
            
            _modelTypeNameDropdown.RegisterValueChangedCallback(HandleModelTypeNameChange);
            _viewTypeNameDropdown.RegisterValueChangedCallback(HandleViewTypeNameChange);
            _uiPrefabObjectField.RegisterValueChangedCallback(HandleUIPrefab);
            _childDropdown.RegisterValueChangedCallback(HandleChildObjectChange);
            _formTypeDropdown.RegisterValueChangedCallback(HandleFormTypeChange);
            _methodDropdown.RegisterValueChangedCallback(HandleModuleMethodChange);
            EventCallback<ChangeEvent<object>> handleSave = _ =>
            {
                EditorUtility.SetDirty(_targetData);
                AssetDatabase.SaveAssetIfDirty(_targetData);
            };
            _uiPrefabObjectField.RegisterCallback(handleSave);
            _typeNameContainer.RegisterCallback(handleSave);
            _formDataContainer.RegisterCallback(handleSave);

            _uiPrefabObjectField.value ??= _targetData.parentPrefab;
            CheckTypeNameContainerActive();
            
            return root;
        }

        private void HandleUIPrefab(ChangeEvent<UnityEngine.Object> evt)
        {
            BasePresenter presenter = evt.newValue as BasePresenter;
            if (presenter == null)
                return;
            
            if (!PrefabUtility.IsPartOfPrefabAsset(presenter.gameObject))
            {
                _targetData.parentPrefab = null;
                _uiPrefabObjectField.SetValueWithoutNotify(null);
                EditorUtility.DisplayDialog("Error", 
                    "프리팹 오브젝트여야 합니다.", "OK");
                return;
            }

            if (evt.newValue != evt.previousValue)
            {
                _targetData.ResetFormData();
                _targetData.modelTypeName = null;
                _targetData.modelTypeName = null;
                _targetData.selectedChildName = null;
                _targetData.parentPrefab = presenter;
            }

            CheckTypeNameContainerActive();
        }

        private void HandleChildObjectChange(ChangeEvent<string> childObjectName)
        {
            _targetData.selectedChildName = childObjectName.newValue;
            UpdateFormDropdowns();
        }
        
        private void HandleModelTypeNameChange(ChangeEvent<string> modelTypeName)
        { 
            _targetData.modelTypeName = modelTypeName.newValue;
            CheckFormContainerActive();
        }

        private void HandleViewTypeNameChange(ChangeEvent<string> viewTypeName)
        {
            _targetData.viewTypeName = viewTypeName.newValue;
            CheckFormContainerActive();
        }

        private void HandleFormTypeChange(ChangeEvent<string> formType)
        {
            CurrentForm.formTypeName = formType.newValue;
        }

        private void HandleModuleMethodChange(ChangeEvent<string> methodName)
        {
            CurrentForm.targetMethodName = methodName.newValue;
        }

        private void CheckTypeNameContainerActive()
        {
            if (_targetData.parentPrefab == null)
            {
                _typeNameContainer.style.display = DisplayStyle.None;
                CheckFormContainerActive();
                return;
            }
            
            FillTypeNameDropdown(_modelTypeNameDropdown, typeof(IModel));
            FillTypeNameDropdown(_viewTypeNameDropdown, typeof(BaseView));
            UpdateTypeNameDropdowns();
            
            _typeNameContainer.style.display = DisplayStyle.Flex;
            
            CheckFormContainerActive();
        }

        private void CheckFormContainerActive()
        {
            if (string.IsNullOrEmpty(_targetData.modelTypeName) || string.IsNullOrEmpty(_targetData.viewTypeName) 
                                      || _targetData.parentPrefab == null)
            {
                _formDataContainer.style.display = DisplayStyle.None;
                return;
            }
            
            FillChildObjectDropdown(_childDropdown);
            if (string.IsNullOrEmpty(_targetData.selectedChildName)
                && _childDropdown.choices.Contains(_targetData.selectedChildName))
            {
                _targetData.selectedChildName = _childDropdown.choices.FirstOrDefault();
            }

            if (CurrentForm != null)
            {
                FillTargetMethodNameDropdown(_methodDropdown);
                FillFormTypeDropdown(_formTypeDropdown);
                UpdateFormDropdowns(); 
            }

            _formDataContainer.style.display = DisplayStyle.Flex;
        }

        private void FillTypeNameDropdown(DropdownField field, Type fieldType)
        {
            IEnumerable<string> choices = EditorInfo.GetTypeNames(fieldType, true);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
            field.SetValueWithoutNotify(null);
        }

        private void FillFormTypeDropdown(DropdownField field)
        {
            IEnumerable<string> choices = EditorInfo.GetTypeNames(typeof(BaseForm), false);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
            field.SetValueWithoutNotify(null);
        }
        
        private void FillChildObjectDropdown(DropdownField field)
        {
            IEnumerable<string> choices = _targetData.parentPrefab.GetComponentsInChildren<Transform>()
                .Where(child => child.gameObject.name.Contains("F_"))
                .Select(child => child.name);

            List<FormData> formList = _targetData.GetFormDataArray().ToList();
            List<FormData> newFormList = new();
            
            foreach (string formName in choices)
            {
                bool shouldAppend = true;
                
                foreach (var form in formList)
                {
                    if (formName == form.gameObjectName)
                    {
                        newFormList.Add(form);
                        shouldAppend = false;
                    }
                }
                
                if (shouldAppend == true)
                {
                    FormData form = new() { gameObjectName = formName };
                    newFormList.Add(form);
                }
            }
            
            _targetData.SetFormDataList(newFormList);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
        }
        
        private void FillTargetMethodNameDropdown(DropdownField field)
        {
            Type dataType = typeof(BaseUIData);
            IEnumerable<string> choices = EditorInfo.UIAssembly.GetTypes()
                .Where(type => type.Name == _targetData.modelTypeName)
                .FirstOrDefault()
                ?.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => method.ReturnType == dataType &&
                                 method.GetParameters()[0].ParameterType == dataType)
                .Select(method => method.Name);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
        }

        private void UpdateTypeNameDropdowns()
        {
            UpdateFieldDropdown(_modelTypeNameDropdown, ref _targetData.modelTypeName);
            UpdateFieldDropdown(_viewTypeNameDropdown, ref _targetData.viewTypeName);
        }

        private void UpdateFormDropdowns()
        {
            if (CurrentForm != null)
            {
                UpdateFieldDropdown(_childDropdown, ref _targetData.selectedChildName);
                UpdateFieldDropdown(_formTypeDropdown, ref CurrentForm.formTypeName);
                UpdateFieldDropdown(_methodDropdown, ref CurrentForm.targetMethodName);
            }
        }
        
        private void UpdateFieldDropdown(DropdownField field, ref string value)
        {
            if (_targetData != null && !string.IsNullOrEmpty(value)
                                    && field.choices.Contains(value))
            {
                field.SetValueWithoutNotify(value);
            } else if (_targetData != null && string.IsNullOrEmpty(value))
            {
                value = null;
                field.SetValueWithoutNotify(null);
                EditorUtility.SetDirty(_targetData);
            }
            
            AssetDatabase.SaveAssetIfDirty(_targetData);
        }
    }
}
