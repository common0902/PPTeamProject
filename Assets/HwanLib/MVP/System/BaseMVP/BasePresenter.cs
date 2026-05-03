using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class BasePresenter : MonoBehaviour
    {
        protected BaseView View;
        protected IModel Model;

        private Dictionary<int, Action<ChangedData>> _moduleInteractMethodDict;
        private Dictionary<int, Func<ChangedData>> _moduleUpdateMethodDict;
        
        public virtual void InitializePresenter(MVPDataSO dataSO)
        {
            List<FormData> formDataList = dataSO.GetFormDataList();
            Dictionary<int, string> formInteractMethodDict = new Dictionary<int, string>();
            Dictionary<int, string> formUpdateMethodDict = new Dictionary<int, string>();
            _moduleInteractMethodDict = new Dictionary<int, Action<ChangedData>>();
            _moduleUpdateMethodDict = new Dictionary<int, Func<ChangedData>>();
            
            foreach (FormData form in formDataList)
            {
                formInteractMethodDict.Add(form.childIndex, form.targetInteractMethodName);
                formUpdateMethodDict.Add(form.childIndex, form.targetUpdateMethodName);
            }
            
            Model = Activator.CreateInstance(dataSO.GetModelType()) as IModel;
            View = Activator.CreateInstance(dataSO.GetViewType()) as BaseView;
            
            if (Model == null || View == null)
            {
                Debug.LogError("Model 혹은 View의 타입을 알 수 없습니다.");
                return;
            }
            
            //Module에서 메서드 찾아서 Delegate로 변환 -> 메서드 이름을 키로 하여 저장
            MethodInfo[] targetMethods = Model.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(method => method.Name.Contains("I_") || method.Name.Contains("U_"))
                .ToArray();

            foreach (KeyValuePair<int, string> targetMethodData in formInteractMethodDict)
            {
                foreach (MethodInfo method in targetMethods)
                {
                    if (method.Name == targetMethodData.Value)
                    {
                        _moduleInteractMethodDict.Add(targetMethodData.Key
                            , method.CreateDelegate(typeof(Action<ChangedData>), Model) as Action<ChangedData>);
                        break;
                    }
                }
            }
            
            foreach (KeyValuePair<int, string> targetMethodData in formUpdateMethodDict)
            {
                foreach (MethodInfo method in targetMethods)
                {
                    if (method.Name == targetMethodData.Value)
                    {
                        _moduleUpdateMethodDict.Add(targetMethodData.Key, 
                            method.CreateDelegate(typeof(Func<ChangedData>), Model) as Func<ChangedData>);
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

        private void InteractedHandler(int childIndex, ChangedData value)
        {
            if (_moduleInteractMethodDict.TryGetValue(childIndex, out Action<ChangedData> interactMethod))
            {
                interactMethod?.Invoke(value);
            }
        }

        private ChangedData UpdateHandler(int childIndex)
        {
            if (_moduleUpdateMethodDict.TryGetValue(childIndex, out Func<ChangedData> updateMethod))
            {
                return updateMethod?.Invoke();
            }

            Debug.LogWarning("메서드가 연결되지 않은 Updatable Form이 Update되었습니다.");
            return null;
        }
    }
}