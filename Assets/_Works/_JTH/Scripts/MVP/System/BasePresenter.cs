using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MVP.System
{
    public abstract class BasePresenter : MonoBehaviour
    {
        private BaseView _view;
        private IModel _model;

        private Dictionary<string, Func<BaseUIData, BaseUIData>> ModuleMethodDict;

        private void Awake()
        {
            _view.InitializeView(gameObject);
            _model.InitializeModule();

            //Module에서 메서드 찾아서 Delegate로 변환 -> 메서드 이름을 키로 하여 저장
            Type dataType = typeof(BaseUIData);
            Type delegateType = typeof(Func<BaseUIData, BaseUIData>);
            MethodInfo[] methodInfo = _model.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo method in methodInfo)
            {
                if ((method.ReturnType == dataType &&
                    method.GetParameters()[0].ParameterType == dataType) == true)
                {
                    ModuleMethodDict.Add(method.Name, 
                        (Func<BaseUIData, BaseUIData>)method.CreateDelegate(delegateType, _model));
                }
            }
        }

        private void OnEnable()
        {
            _view.OnInteractiveObject += ApplyChangedValue;
        }
        
        private BaseUIData ApplyChangedValue(string propertieName, BaseUIData value)
        {
            return ModuleMethodDict[propertieName]?.Invoke(value);
        }
    }
}