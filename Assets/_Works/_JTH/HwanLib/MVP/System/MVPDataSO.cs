using System;
using System.Collections.Generic;
using UnityEngine;

namespace HwanLib.MVP.System
{
    [CreateAssetMenu(fileName = "Event Container DataSO", menuName = "UI/MVP/Event Container DataSO")]
    public class MVPDataSO : ScriptableObject
    {
        [HideInInspector] public BasePresenter parentPrefab;
        [ReadOnly] public List<FormData> formDataList;

        public string modelTypeName { get; private set; }
        public string viewTypeName { get; private set; }
        public Type modelType { get; private set; }
        public Type viewType { get; private set; }

        public void SetType(bool isModelType, Type type, string name)
        {
            if (isModelType == true)
            {
                modelType = type;
                modelTypeName = name;
            }
            else
            {
                viewType = type;
                viewTypeName = name;
            }
        }
        
        #if UNITY_EDITOR
        [HideInInspector] public int selectedChildIndex = 0;
        #endif
    }
}
