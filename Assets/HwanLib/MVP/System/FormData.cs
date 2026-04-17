using System;

namespace HwanLib.MVP.System
{
    [Serializable]
    public class FormData
    {
        public int childIndex;
        public string targetMethodName;
        public string formTypeName;
        public string gameObjectName;

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
