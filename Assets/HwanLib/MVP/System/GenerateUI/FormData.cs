using System;
using System.Linq;
using System.Reflection;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System.BaseMVP.Form;
using UnityEngine;

namespace HwanLib.MVP.System.GenerateUI
{
    [Serializable]
    public class FormData
    {
        [HideInInspector] public int childIndex;
        public string gameObjectName;
        public string formTypeName;
        public string targetInteractMethodName;
        public string targetUpdateMethodName;
        [HideInInspector] public bool isInteractable;
        [HideInInspector] public bool isUpdatable;

        public FormData()
        {
            childIndex = -1;
            gameObjectName = "";
            targetInteractMethodName = "";
            targetUpdateMethodName = "";
            formTypeName = "";
        }
        
        public FormData(int childIndex, string gameObjectName, string defaultForm, string defaultMethod)
        {
            this.childIndex = childIndex;
            this.gameObjectName = gameObjectName;
            formTypeName = defaultForm;
            targetInteractMethodName = defaultMethod;
            targetUpdateMethodName = defaultMethod;
        }

        public void SetBool()
        {
            isInteractable = false;
            isUpdatable = false;

            if (String.IsNullOrEmpty(formTypeName))
                return;

            //Form이 있는 Assembly에서 검색
            Type formType = Assembly.GetAssembly(typeof(AccessForm)).GetTypes()
                .Single(type => type.Name == formTypeName);
            isInteractable = typeof(IInteractable).IsAssignableFrom(formType);
            isUpdatable = typeof(IUpdatable).IsAssignableFrom(formType);
        }
    }
}
