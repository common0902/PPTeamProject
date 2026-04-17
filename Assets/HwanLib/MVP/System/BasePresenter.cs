using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BasePresenter : MonoBehaviour
    {
        protected BaseView View;
        protected IModel Model;

        private Dictionary<string, FormData> _formDataDict;
        private Dictionary<string, Func<ChangedData, ChangedData>> _moduleMethodDict;

        public virtual void InitializePresenter(MVPDataSO dataSO)
        {
            List<FormData> formDataList = dataSO.GetFormDataList();
            foreach (FormData form in formDataList)
            {
                _formDataDict.Add(form.gameObjectName, form);
            }

            if (!string.IsNullOrEmpty(dataSO.modelTypeName))
                InitModel(dataSO.GetModelType());
            
            InitView(dataSO.GetViewType(), formDataList);
        }

        private void InitModel(Type modelType)
        {
            Model = Activator.CreateInstance(modelType) as IModel;
            
            Model?.InitializeData();
            
            //Module에서 메서드 찾아서 Delegate로 변환 -> 메서드 이름을 키로 하여 저장
            Type dataType = typeof(ChangedData);
            Type delegateType = typeof(Func<ChangedData, ChangedData>);
            MethodInfo[] methodInfo = Model.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public);
            
            foreach (MethodInfo method in methodInfo)
            {
                if ((method.ReturnType == dataType &&
                     method.GetParameters()[0].ParameterType == dataType) == true)
                {
                    // MehtodInfo엔 구현에 대한 정보도 없고, 메서드에서 객체의 필드를
                    // 사용할 수도 있기 때문에 클래스의 객체를 넣어주어야 한다.
                    _moduleMethodDict.Add(method.Name, 
                        method.CreateDelegate(delegateType, Model) as Func<ChangedData, ChangedData>);
                }
            }
        }

        private void InitView(Type viewType, List<FormData> formDataList)
        {
            View = Activator.CreateInstance(viewType) as BaseView;
            
            View.InitializeView(gameObject, formDataList);
        }

        protected virtual void OnEnable()
        {
            View.OnInteractObject += ApplyChangedValue;
        }

        protected virtual void OnDisable()
        {
            View.OnInteractObject -= ApplyChangedValue;
        }

        private ChangedData ApplyChangedValue(in string senderName, ChangedData value)
        {
            string methodName = _formDataDict[senderName].targetMethodName;
            return _moduleMethodDict[methodName]?.Invoke(value);
        }
    }
}