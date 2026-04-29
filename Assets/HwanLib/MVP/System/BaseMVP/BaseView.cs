using System;
using System.Collections.Generic;
using HwanLib.MVP.System.AddFormComponent;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class BaseView
    {
        protected Canvas RootCanvas { get; private set; } 
        private Action<int> _viewEvent;

        //Dict로 접근할 수는 있지만, 접근하면 캡슐화와 일관성이 깨지기 때문에 UpdateVisual로만 Form의 Visual을 바꿀 수 있게 함.
        private Dictionary<int, BaseForm> _formDict;
        private Dictionary<(Action, int), Action<int>> _lookup;

        public virtual void InitializeView(GameObject root, List<FormData> formDataList
            , FormInteracted formInteractedHandler)
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
                
                form.OnFormInteracted += InvokeViewEvent;
                form.OnFormInteracted += formInteractedHandler;
                
                _formDict.Add(formData.childIndex, form);
            }
            
            RootCanvas.gameObject.SetActive(false);
        }

        public virtual void OpenView()
        {
            foreach (var form in _formDict.Values)
            {
                form.UpdateVisual();
            }
            RootCanvas.gameObject.SetActive(true);
        }

        public virtual void CloseView()
        {
            RootCanvas.gameObject.SetActive(false);
        }

        public virtual void OnDestroyView()
        {
            
        }
        
        protected T GetForm<T>(int childEnum) where T : BaseForm
        {
            return _formDict[childEnum] as T;
        }

        protected void AddListener(Action handler, int childEnum)
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

        protected void RemoveListener(Action handler, int childEnum)
        {
            (Action action, int childIndex) handlerData = (handler, childEnum);
            
            if (!_lookup.TryGetValue(handlerData, out Action<int> wrappedHandler))
                return;

            _viewEvent -= wrappedHandler;
        }

        private ChangedData InvokeViewEvent(int childIndex, ChangedData _)
        {
            if (_ == null)
                return null;
            
            _viewEvent?.Invoke(childIndex);
            return null;
        }
    }
}