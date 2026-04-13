using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVP.System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MVP.Editor
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
        private Dictionary<string, int> _formIndexDict;
        private Assembly _uiAssembly;

        private Assembly UIAssembly
        {
            get
            {
                if (_uiAssembly == null)
                {
                    Type targetType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(ass => ass.GetTypes())
                        .Where(type => type.Name == "MVPAssemblyMarker")
                        .FirstOrDefault();
            
                    if (targetType == null)
                        EditorUtility.DisplayDialog("Error",
                            "타입을 찾기 위해 MVPAssemblyMarker.cs 파일이 필요합니다.", "OK");
            
                    _uiAssembly = Assembly.GetAssembly(targetType);
                }

                return _uiAssembly;
            }
        }
        
        private FormData CurrentFormData
        {
            get
            {
                if (_targetData.formDataList.Count == 0)
                    return null;
                return _targetData.formDataList[_targetData.selectedChildIndex];
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
            EventCallback<ChangeEvent<object>> handleSave = (ChangeEvent<object> _) =>
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
                _targetData.formDataList = null;
                _targetData.modelTypeName = null;
                _targetData.viewTypeName = null;
                _targetData.parentPrefab = presenter;
            }

            CheckTypeNameContainerActive();
        }

        private void HandleChildObjectChange(ChangeEvent<string> childObjectName)
        {
            _targetData.selectedChildIndex = _formIndexDict[childObjectName.newValue];
            
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
            CurrentFormData.formType = formType.newValue;
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

            if (CurrentFormData != null)
            {
                FillTargetMethodNameDropdown(_methodDropdown);
                FillFormTypeDropdown(_formTypeDropdown);
                UpdateFormDropdowns();
            }
            
            _formDataContainer.style.display = DisplayStyle.Flex;
        }

        private void HandleModuleMethodChange(ChangeEvent<string> methodName)
        {
            CurrentFormData.targetMethodName = methodName.newValue;
        }

        private void FillTypeNameDropdown(DropdownField field, Type fieldType)
        {
            IEnumerable<string> choices = GetTypeNames(fieldType);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
            field.SetValueWithoutNotify(null);
        }

        private void FillFormTypeDropdown(DropdownField field)
        {
            IEnumerable<string> choices = GetTypeNames(typeof(InteractiveObject));
            
            field.choices.Clear();
            field.choices.AddRange(choices);
            field.SetValueWithoutNotify(null);
        }
        
        private void FillChildObjectDropdown(DropdownField field)
        {
            _targetData.formDataList ??= new List<FormData>();
            _formIndexDict = new Dictionary<string, int>();
            
            IEnumerable<string> choices = _targetData.parentPrefab.GetComponentsInChildren<Transform>()
                .Where(child => child.gameObject.name.Contains("F_"))
                .Select(child =>
                {
                    if (_targetData.formDataList
                        .TrueForAll(data => data.formObject != child.gameObject))
                    {
                        FormData formData = new FormData();
                        _targetData.formDataList.Add(formData);
                        formData.formObject = child.gameObject;
                    }
                    int idx = _targetData.formDataList.Count - 1;
                    _formIndexDict.Add(_targetData.formDataList[idx].formObject.name, idx);
                    return child.name;
                });
            
            List<string> childNamesList = new List<string>();
                
            foreach (string choice in _formIndexDict.Keys)
            {
                if (!childNamesList.Contains(choice))
                    childNamesList.Add(choice);
                else
                {
                    EditorUtility.DisplayDialog("Error",
                        "같은 이름의 인터렉티브 오브젝트가 2개 이상 존재합니다.", "OK");
                    _targetData.parentPrefab = null;
                    _uiPrefabObjectField.SetValueWithoutNotify(null);
            
                    return;
                }
            }

            field.choices.Clear();
            field.choices.AddRange(choices);
            field.SetValueWithoutNotify(null);
        }

        private void FillTargetMethodNameDropdown(DropdownField field)
        {
            Type dataType = typeof(BaseUIData);

            IEnumerable<string> choices = UIAssembly.GetTypes()
                .Where(type => type.Name == _targetData.modelTypeName)
                .FirstOrDefault()
                ?.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => method.ReturnType == dataType &&
                                 method.GetParameters()[0].ParameterType == dataType)
                .Select(method => method.Name);

            if (choices.Count() == 0)
                EditorUtility.DisplayDialog("Error",
                    "모듈에 메서드가 존재하지 않습니다.", "OK");
            
            field.choices.Clear();
            field.choices.AddRange(choices);
            field.SetValueWithoutNotify(null);
        }

        private void UpdateTypeNameDropdowns()
        {
            UpdateFieldDropdown(_modelTypeNameDropdown, ref _targetData.modelTypeName);
            UpdateFieldDropdown(_viewTypeNameDropdown, ref _targetData.viewTypeName);
        }

        private void UpdateFormDropdowns()
        {
            if (_targetData != null && 
                
                _childDropdown.choices.Contains(CurrentFormData.formObject.name))
            {
                _childDropdown.SetValueWithoutNotify(CurrentFormData.formObject.name);
            } else if (_targetData != null && _targetData.selectedChildIndex != 0)
            {
                _targetData.selectedChildIndex = 0;
                _childDropdown.SetValueWithoutNotify(CurrentFormData.formObject.name);
                EditorUtility.SetDirty(_targetData);
            }
            
            AssetDatabase.SaveAssetIfDirty(_targetData);

            if (CurrentFormData != null)
            {
                UpdateFieldDropdown(_methodDropdown, ref CurrentFormData.targetMethodName);
                UpdateFieldDropdown(_formTypeDropdown, ref CurrentFormData.formType);
            }
        }
        
        private void UpdateFieldDropdown(DropdownField field, ref string value)
        {
            if (_targetData != null && !string.IsNullOrEmpty(value)
                                    && field.choices.Contains(value))
            {
                field.SetValueWithoutNotify(value);
            } else if (_targetData != null)
            {
                value = null;
                EditorUtility.SetDirty(_targetData);
            }
            
            AssetDatabase.SaveAssetIfDirty(_targetData);
        }

        private IEnumerable<string> GetTypeNames(Type baseType)
        {
            IEnumerable<string> choices = UIAssembly.GetTypes()
                .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract 
                               && baseType.IsAssignableFrom(type))
                .Select(type => type.Name);
            
            return choices;
        }
    }
}
