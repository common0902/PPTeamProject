using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AddFormComponent;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.System.GenerateUI;
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
        private DropdownField _interactMethodDropdown;
        private DropdownField _updateMethodDropdown;
        private VisualElement _typeNameContainer;
        private VisualElement _formDataContainer;

        private GenerateScriptEditor _scriptGenerator;
        private const string _defaultMethodsName = "None";

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
            _interactMethodDropdown = root.Q<DropdownField>("TargetInteractMethodNameDropdown");
            _updateMethodDropdown = root.Q<DropdownField>("TargetUpdateMethodNameDropdown");
            _uiPrefabObjectField = root.Q<ObjectField>("UIPrefabObjectField");
            _typeNameContainer = root.Q<VisualElement>("TypeNameContainer");
            _formDataContainer = root.Q<VisualElement>("FormDataContainer");
            
            _modelTypeNameDropdown.RegisterValueChangedCallback(HandleModelTypeNameChange);
            _viewTypeNameDropdown.RegisterValueChangedCallback(HandleViewTypeNameChange);
            _uiPrefabObjectField.RegisterValueChangedCallback(HandleUIPrefab);
            _childDropdown.RegisterValueChangedCallback(HandleChildObjectChange);
            _formTypeDropdown.RegisterValueChangedCallback(HandleFormTypeChange);
            _interactMethodDropdown.RegisterValueChangedCallback(HandleInteractMethodChange);
            _updateMethodDropdown.RegisterValueChangedCallback(HandleUpdateMethodChange);

            if (_targetData.parentPrefab != null)
            {
                _uiPrefabObjectField.SetValueWithoutNotify(_targetData.parentPrefab);
                CheckTypeNameContainerActive();
                
                _scriptGenerator = new GenerateScriptEditor(root, _targetData.parentPrefab.name + "Enum"
                    , _targetData.parentPrefab.name + "EnumPath", GenerateCode);
            }
            else
            {
                _scriptGenerator = new GenerateScriptEditor(root);
                CheckTypeNameContainerActive();
            }
            
            return root;
        }

        private void HandleUIPrefab(ChangeEvent<UnityEngine.Object> evt)
        {
            BasePresenter presenter = evt.newValue as BasePresenter;
            if (presenter == null)
            {
                _targetData.parentPrefab = null;
                CheckTypeNameContainerActive();
                return;
            }
            
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

            _scriptGenerator.FileName = _targetData.parentPrefab.name + "Form";
            _scriptGenerator.PrefsName = _scriptGenerator.FileName + "Path";

            CheckTypeNameContainerActive();
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

        private void HandleChildObjectChange(ChangeEvent<string> childObjectName)
        {
            _targetData.selectedChildName = childObjectName.newValue;
            UpdateFormDropdowns();
        }

        private void HandleFormTypeChange(ChangeEvent<string> formType)
        {
            CurrentForm.formTypeName = formType.newValue;
            SetTargetMethodsDisplayStyle();
        }

        private void SetTargetMethodsDisplayStyle()
        {
            CurrentForm.SetBool();
            
            if (CurrentForm.isInteractable)
                _interactMethodDropdown.style.display = DisplayStyle.Flex;
            else
            {
                _interactMethodDropdown.style.display = DisplayStyle.None;
                CurrentForm.targetInteractMethodName = _defaultMethodsName;
            }

            if (CurrentForm.isUpdatable)
            {
                _updateMethodDropdown.style.display = DisplayStyle.Flex;
                _updateMethodDropdown.choices = _updateMethodDropdown.choices
                    .Replace(_defaultMethodsName, "Not Selected")
                    .ToList();
                if (CurrentForm.targetUpdateMethodName == _defaultMethodsName)
                    CurrentForm.targetUpdateMethodName = "Not Selected";
                UpdateFieldDropdown(_updateMethodDropdown
                    , ref CurrentForm.targetUpdateMethodName
                    , "Not Selected");
            }
            else
            {
                _updateMethodDropdown.style.display = DisplayStyle.None;
                CurrentForm.targetUpdateMethodName = _defaultMethodsName;
                _updateMethodDropdown.choices = _updateMethodDropdown.choices
                    .Replace("Not Selected", _defaultMethodsName)
                    .ToList();    
                UpdateFieldDropdown(_updateMethodDropdown
                    , ref CurrentForm.targetUpdateMethodName
                    , _defaultMethodsName);
            }
        }

        private void HandleInteractMethodChange(ChangeEvent<string> methodName)
        {
            CurrentForm.targetInteractMethodName = methodName.newValue;
        }
        
        private void HandleUpdateMethodChange(ChangeEvent<string> methodName)
        {
            CurrentForm.targetUpdateMethodName = methodName.newValue;
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
            
            SetDefaultName();
            UpdateTypeNameDropdowns();
            
            _typeNameContainer.style.display = DisplayStyle.Flex;
            
            CheckFormContainerActive();
        }

        private void CheckFormContainerActive()
        {
            //Model은 간단한 UI에선 사용하지 않을 수 있기 때문에 검사에서 제외
            if (string.IsNullOrEmpty(_targetData.viewTypeName)
                || string.IsNullOrEmpty(_targetData.modelTypeName) 
                || _targetData.parentPrefab == null)
            {
                _formDataContainer.style.display = DisplayStyle.None;
                return;
            }

            FillChildObjectDropdown(_childDropdown);
            if (string.IsNullOrEmpty(_targetData.selectedChildName) 
                || !_childDropdown.choices.Contains(_targetData.selectedChildName))
            {
                _targetData.selectedChildName = _childDropdown.choices.FirstOrDefault();
            }

            if (CurrentForm != null)
            {
                FillTargetMethodsNameDropdown(_interactMethodDropdown, _updateMethodDropdown);
                FillFormTypeDropdown(_formTypeDropdown);
                UpdateFormDropdowns();
            }
            
            _formDataContainer.style.display = DisplayStyle.Flex;
        }

        private void FillTypeNameDropdown(DropdownField field, Type fieldType)
        {
            IEnumerable<string> choices = MVPEditorUtil.GetTypeNamesInUIAssembly(fieldType);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
        }

        private void FillChildObjectDropdown(DropdownField field)
        {
            Transform[] children = _targetData.parentPrefab.GetComponentsInChildren<Transform>();
            
            List<FormData> formList = _targetData.GetFormDataList();
            List<FormData> newFormList = new();
            
            for (int i = 0; i < children.Length; ++i)
            {
                Transform child = children[i];
                if (child.gameObject.name.Contains("F_") == false)
                    continue;
                
                bool shouldAppend = true;
                
                foreach (var form in formList)
                {
                    if (child.name == form.gameObjectName)
                    {
                        form.childIndex = i;
                        newFormList.Add(form);
                        shouldAppend = false;
                    }
                }
                
                if (shouldAppend == true)
                {
                    FormData form = new(i, child.name, "AccessForm", _defaultMethodsName);
                    newFormList.Add(form);
                }
            }

            _targetData.SetFormDataList(newFormList);
            
            IEnumerable<string> choices = newFormList
                .Select(data => data.gameObjectName);
            
            field.choices.Clear();
            field.choices.AddRange(choices);
        }
        
        private void FillFormTypeDropdown(DropdownField field)
        {
            Assembly formAssembly = Assembly.GetAssembly(typeof(AccessForm));
            IEnumerable<string> choices = MVPEditorUtil.GetAssignedTypesInAssembly(typeof(BaseForm), formAssembly)
                .Select(type => type.Name);

            field.choices.Clear();
            field.choices.AddRange(choices);
        }
        
        private void FillTargetMethodsNameDropdown(DropdownField interactField, DropdownField updateField)
        {
            IEnumerable<MethodInfo> methods = MVPEditorUtil.GetTypesInUIAssembly(typeof(IModel))
                .SingleOrDefault(type => type.Name == _targetData.modelTypeName)
                ?.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(method => !method.IsAnonymous());

            IEnumerable<string> interactChoices = methods
                ?.Where(method =>
                    {
                        if (method.ReturnType == typeof(void)
                            && method.GetParameters().Length == 1
                            && method.GetParameters()[0].ParameterType == typeof(UIParam)) return true;
                            
                        return false;
                    }
                )
                .Select(method => method.Name);
            
            IEnumerable<string> updateChoice = methods
                ?.Where(method =>
                    {
                        if (method.ReturnType == typeof(UIParam)
                            && method.GetParameters().Length == 0) return true;

                        return false;
                    }
                )
                .Select(method => method.Name);

            // 중복 제거는 자동으로 됨.
            interactField.choices.Clear();
            interactField.choices.Add(_defaultMethodsName);
            interactField.choices.AddRange(interactChoices);
            
            updateField.choices.Clear();
            updateField.choices.Add(_defaultMethodsName);
            updateField.choices.AddRange(updateChoice);
        }
        
        private void SetDefaultName()
        {
            if (_targetData.parentPrefab == null)
                return;

            //실제로 존재하지 않아도 Update할 때 자동으로 걸러짐
            if (string.IsNullOrEmpty(_targetData.viewTypeName))
            {
                _targetData.viewTypeName = _targetData.parentPrefab.GetType()
                    .Name.Replace("Presenter", "View");
            }
            if (string.IsNullOrEmpty(_targetData.modelTypeName))
            {
                _targetData.modelTypeName = _targetData.parentPrefab.GetType()
                    .Name.Replace("Presenter", "Model");
            }
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
                UpdateFieldDropdown(_formTypeDropdown, ref CurrentForm.formTypeName, "AccessForm");

                SetTargetMethodsDisplayStyle();
                UpdateFieldDropdown(_interactMethodDropdown, ref CurrentForm.targetInteractMethodName, _defaultMethodsName);
                UpdateFieldDropdown(_updateMethodDropdown, ref CurrentForm.targetUpdateMethodName, _defaultMethodsName);
            }
        }
        
        private void UpdateFieldDropdown(DropdownField field, ref string value, string defaultValue = null)
        {
            if (_targetData != null && !string.IsNullOrEmpty(value)
                                    && field.choices.Contains(value))
            {
                field.SetValueWithoutNotify(value);
            }
            else if (_targetData != null && !string.IsNullOrEmpty(defaultValue)
                                         && field.choices.Contains(defaultValue))
            {
                if (defaultValue == "Not Selected")
                    Debug.Log(!string.IsNullOrEmpty(value));
                value = defaultValue;
                field.SetValueWithoutNotify(value);
            }
            else if (_targetData != null)
            {
                value = null;
                field.SetValueWithoutNotify(value);
            }
            
            EditorUtility.SetDirty(_targetData);
            AssetDatabase.SaveAssetIfDirty(_targetData);
        }

        private string GenerateCode(string folderPath, string scriptName)
        {
            string enumString = string.Join(", ", _targetData.GetFormDataList()
                .Select(form => $"{form.gameObjectName.Substring("F_".Length)} = {form.childIndex}"));

            string nameSpace = FileUtil.GetProjectRelativePath(folderPath).Substring("Assets/".Length);
            nameSpace = nameSpace.Replace("/", ".");

            return string.Format(CodeFormat.EnumFormat, nameSpace, scriptName, enumString);
        }
    }
}
