using System;
using System.Collections.Generic;
using System.Reflection;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class BasePresenter : MonoBehaviour
    {
        protected BaseView View;
        protected IModel Model;

        private Dictionary<int, string> _formTargetMethodDict;
        private Dictionary<string, Func<ChangedData, ChangedData>> _moduleMethodDict;
        private Dictionary<string, Func<ChangedData>> _moduleUpdateMethodDict;
        
        public virtual void InitializePresenter(MVPDataSO dataSO)
        {
            List<FormData> formDataList = dataSO.GetFormDataList();
            _formTargetMethodDict = new Dictionary<int, string>();
            _moduleMethodDict = new Dictionary<string, Func<ChangedData, ChangedData>>();
            _moduleUpdateMethodDict = new Dictionary<string, Func<ChangedData>>();
            
            foreach (FormData form in formDataList)
            {
                _formTargetMethodDict.Add(form.childIndex, form.targetMethodName);
            }
            
            Model = Activator.CreateInstance(dataSO.GetModelType()) as IModel;
            View = Activator.CreateInstance(dataSO.GetViewType()) as BaseView;
            
            if (Model == null || View == null)
            {
                Debug.LogError("Model 혹은 View의 타입을 알 수 없습니다.");
                return;
            }
            
            //Module에서 메서드 찾아서 Delegate로 변환 -> 메서드 이름을 키로 하여 저장
            Type dataType = typeof(ChangedData);
            MethodInfo[] methodInfo = Model.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            
            foreach (MethodInfo method in methodInfo)
            {
                if ((method.ReturnType == dataType 
                     && method.GetParameters().Length == 1
                     && method.GetParameters()[0].ParameterType == dataType) == true)
                {
                    // MethodInfo엔 구현에 대한 정보도 없고, 메서드에서 객체의 필드를
                    // 사용할 수도 있기 때문에 클래스의 객체를 넣어주어야 한다.
                    _moduleMethodDict.Add(method.Name, 
                        method.CreateDelegate(typeof(Func<ChangedData, ChangedData>), Model) as Func<ChangedData, ChangedData>);
                }
                else if (method.ReturnType == dataType 
                          && method.GetParameters().Length == 0)
                {
                    _moduleUpdateMethodDict.Add(method.Name, 
                        method.CreateDelegate(typeof(Func<ChangedData>), Model) as Func<ChangedData>);
                }
            }

            View.InitializeView(gameObject, formDataList, ApplyChangedValue);
        }

        protected virtual void OnDestroy()
        {
            View.OnDestroyView();
        }

        private ChangedData ApplyChangedValue(int childIndex, ChangedData value)
        {
            string methodName = _formTargetMethodDict[childIndex];
            if (methodName == "None")
                return null;

            if (value == null)
            {
                if (_moduleUpdateMethodDict.TryGetValue(methodName, out Func<ChangedData> func)) 
                    return func.Invoke();
                
                return null;
            }
                    
            return _moduleMethodDict[methodName]?.Invoke(value);
        }
    }
}