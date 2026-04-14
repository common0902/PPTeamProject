using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BasePresenter : MonoBehaviour
    {
        [SerializeField] private MVPDataSO dataSO;
        private BaseView _view;
        private IModel _model;

        private Dictionary<string, Func<BaseUIData, BaseUIData>> ModuleMethodDict;

        private void Awake()
        {
            GenerateViewAndModel();

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
                    // MehtodInfo엔 구현에 대한 정보가 없기 때문에 메서드가 있는 클래스의 객체를 넣어주어야 한다.
                    ModuleMethodDict.Add(method.Name, 
                        (Func<BaseUIData, BaseUIData>)method.CreateDelegate(delegateType, _model));
                }
            }
        }

        private void OnEnable()
        {
            _view.OnInteractiveObject += ApplyChangedValue;
        }

        private void OnDisable()
        {
            _view.OnInteractiveObject -= ApplyChangedValue;
        }

        private void GenerateViewAndModel()
        {
            _view = Activator.CreateInstance(dataSO.viewType) as BaseView;
            _model = Activator.CreateInstance(dataSO.modelType) as IModel;
            
            _view.InitializeView(gameObject, dataSO.formDataList);
            _model.InitializeModule();
        }

        private BaseUIData ApplyChangedValue(GameObject sender, BaseUIData value)
        {
            // string methodName = dataSO.formDataList[]
            return ModuleMethodDict[sender.name]?.Invoke(value);
        }
    }
}