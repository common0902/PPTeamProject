using System;
using UnityEngine;

namespace HwanLib.MVP.System
{
    [Serializable]
    public class FormData
    {
        [HideInInspector] public int childIndex;
        public string gameObjectName;
        public string formTypeName;
        public string targetMethodName;

        public FormData()
        {
            childIndex = -1;
            gameObjectName = "";
            targetMethodName = "";
            formTypeName = "";
        }
        
        public FormData(int childIndex, string gameObjectName)
        {
            this.childIndex = childIndex;
            this.gameObjectName = gameObjectName;
        }
    }
}
