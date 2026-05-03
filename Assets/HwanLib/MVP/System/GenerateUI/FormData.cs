using System;
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
    }
}
