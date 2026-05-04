using System;
using System.Collections.Generic;
using System.Reflection;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class BasePresenter : MonoBehaviour
    {
        protected BaseView View;
        protected IModel Model;

        private Dictionary<int, Action<UIParam>> _moduleInteractMethodDict;
        private Dictionary<int, Func<UIParam>> _moduleUpdateMethodDict;
        
        public virtual void InitializePresenter(MVPDataSO dataSO)
        {
            List<FormData> formDataList = dataSO.GetFormDataList();
            Dictionary<int, string> formInteractMethodDict = new Dictionary<int, string>();
            Dictionary<int, string> formUpdateMethodDict = new Dictionary<int, string>();
            _moduleInteractMethodDict = new Dictionary<int, Action<UIParam>>();
            _moduleUpdateMethodDict = new Dictionary<int, Func<UIParam>>();
            
            foreach (FormData form in formDataList)
            {
                if (form.isInteractable)
                    formInteractMethodDict.Add(form.childIndex, form.targetInteractMethodName);
                if (form.isUpdatable)
                    formUpdateMethodDict.Add(form.childIndex, form.targetUpdateMethodName);
            }
            
            Model = Activator.CreateInstance(dataSO.GetModelType()) as IModel;
            View = Activator.CreateInstance(dataSO.GetViewType()) as BaseView;
            
            if (Model == null || View == null)
            {
                Debug.LogError("Model 혹은 View의 타입을 알 수 없습니다.");
                return;
            }
            
            MethodInfo[] targetMethods = Model.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (KeyValuePair<int, string> targetMethodData in formInteractMethodDict)
            {
                foreach (MethodInfo method in targetMethods)
                {
                    if (method.Name == targetMethodData.Value
                        && method.ReturnType == typeof(void)
                        && method.GetParameters().Length == 1
                        && method.GetParameters()[0].ParameterType == typeof(UIParam))
                    {
                        _moduleInteractMethodDict.Add(targetMethodData.Key
                            , method.CreateDelegate(typeof(Action<UIParam>), Model) as Action<UIParam>);
                        break;
                    }
                }
            }
            
            foreach (KeyValuePair<int, string> targetMethodData in formUpdateMethodDict)
            {
                if (targetMethodData.Value == "Not Selected") 
                    Debug.LogWarning($"Updatable을 상속한 Form이 메서드와 연결되어 있지 않습니다. ChildIndex: {targetMethodData.Key}");
                
                foreach (MethodInfo method in targetMethods)
                {
                    if (method.Name == targetMethodData.Value
                        && method.ReturnType == typeof(UIParam)
                        && method.GetParameters().Length == 0)
                    {
                        _moduleUpdateMethodDict.Add(targetMethodData.Key, 
                            method.CreateDelegate(typeof(Func<UIParam>), Model) as Func<UIParam>);
                        break;
                    }
                }
            }

            View.InitializeView(gameObject, formDataList, InteractedHandler, UpdateHandler);
        }

        protected virtual void OnDestroy()
        {
            View.OnDestroyView();
        }

        private void InteractedHandler(int childIndex, UIParam value)
        {
            if (_moduleInteractMethodDict.TryGetValue(childIndex, out Action<UIParam> interactMethod))
            {
                interactMethod?.Invoke(value);
            }
        }

        private UIParam UpdateHandler(int childIndex)
        {
            if (_moduleUpdateMethodDict.TryGetValue(childIndex, out Func<UIParam> updateMethod))
            {
                return updateMethod?.Invoke();
            }
            
            // Interact 이벤트는 View에서 사용할 수도 있지만 Update 이벤트는 무조건 Model과 연결되어 있어야한다.
            Debug.LogWarning($"메서드가 연결되지 않은 Updatable Form이 Update되었습니다. ChildIndex: {childIndex}");
            return null;
        }
    }
}