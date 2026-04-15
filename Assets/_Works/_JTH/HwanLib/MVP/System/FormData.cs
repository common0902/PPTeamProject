using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HwanLib.MVP.System
{
    [Serializable]
    public class FormData
    {
        public string gameObjectName;
        public string targetMethodName;
        public string formTypeName;

        public FormData()
        {
            gameObjectName = "";
            targetMethodName = "";
            formTypeName = "";
        }
        
        public FormData(string gameObjectName, string targetMethodName, string formTypeName)
        {
            this.gameObjectName = gameObjectName;
            this.targetMethodName = targetMethodName;
            this.formTypeName = formTypeName;
        }
    }
}
