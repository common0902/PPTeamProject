using System;
using System.Collections.Generic;
using HwanLib.MVP.System.AddFormComponent;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class BaseView
    {
        protected Canvas RootCanvas { get; private set; } 
        private Action<int> _viewEvent;

        private Dictionary<int, BaseForm> _formDict;
        private Dictionary<(Action, int), Action<int>> _lookup;

        public virtual void InitializeView(GameObject root, List<FormData> formDataList
            , FormInteracted formInteractedHandler, UpdateForm updateFormHandler)
        {
            RootCanvas = root.transform.GetChild(0).GetComponent<Canvas>();
            _formDict = new Dictionary<int, BaseForm>();
            _lookup = new Dictionary<(Action, int), Action<int>>();
            
            Transform[] children = root.GetComponentsInChildren<Transform>();
            foreach (var formData in formDataList)
            {
                Transform child = children[formData.childIndex];
                
                BaseForm form = child.gameObject
                    .AddFormComponent(formData.formTypeName);
                form.InitializeForm(formData.childIndex);

                if (form is IInteractable interactable)
                {
                    interactable.OnFormInteracted += OnFormInteract;
                    interactable.OnFormInteracted += formInteractedHandler;
                }

                if (form is IUpdatable updatable)
                {
                    updatable.OnFormUpdate += updateFormHandler;
                }
                
                _formDict.Add(formData.childIndex, form);
            }
            
            RootCanvas.gameObject.SetActive(false);
        }

        public virtual void OpenView()
        {
            UpdateView();
            RootCanvas.gameObject.SetActive(true);
        }

        public virtual void OnDestroyView()
        {
            
        }

        public virtual void UpdateView()
        {
            foreach (var form in _formDict.Values)
            {
                if (form is IUpdatable updatable)
                {
                    updatable.UpdateForm();
                }
            }
        }
        
        protected T GetForm<T>(int childEnum) where T : BaseForm
        {
            return _formDict[childEnum] as T;
        }

        protected void AddFormInteractionListener(Action handler, int childEnum)
        {
            (Action action, int childIndex) handlerData = (handler, childEnum);
            if (_lookup.ContainsKey(handlerData) == true)
                return;

            Action<int> wrappedHandler = childIndex =>
            {
                if (childIndex == childEnum) handlerData.action.Invoke();
            };
            _viewEvent += wrappedHandler;
            _lookup.Add(handlerData, wrappedHandler);
        }

        protected void RemoveFormInteractionListener(Action handler, int childEnum)
        {
            (Action action, int childIndex) handlerData = (handler, childEnum);
            
            if (!_lookup.TryGetValue(handlerData, out Action<int> wrappedHandler))
                return;

            _viewEvent -= wrappedHandler;
        }

        private void OnFormInteract(int childIndex, ChangedData _)
            => _viewEvent?.Invoke(childIndex);

        private ChangedData OnFormUpdate(int childIndex)
        {
            _viewEvent?.Invoke(childIndex);
            return null;
        }
    }
}