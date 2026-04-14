using System;
using System.Reflection;
using UnityEngine;

namespace HwanLib.MVP.System
{
    [Serializable]
    public class FormData
    { 
        public GameObject formObject; 
        public string targetMethodName;
        public string formTypeName;
        public Type formType;
        public Func<GameObject, BaseForm> addComponentMethod;
        
        public void SetType(Type type, string name)
        {
            formType = type;
            formTypeName = name;
            
            BaseForm form = Activator.CreateInstance(formType) as BaseForm;
            MethodInfo method = formType.GetMethod("AddComponentToObject"
                , BindingFlags.Public | BindingFlags.Static);
            
            // MehtodInfo엔 구현에 대한 정보가 없기 때문에 메서드가 있는 클래스의 객체를 넣어주어야 한다.
            addComponentMethod = method
                .CreateDelegate(typeof(Func<GameObject, BaseForm>), form) as Func<GameObject, BaseForm>;
        }
    }
}
