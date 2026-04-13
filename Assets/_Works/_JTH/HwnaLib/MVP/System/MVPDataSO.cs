using System.Collections.Generic;
using UnityEngine;

namespace MVP.System
{
    [CreateAssetMenu(fileName = "Event Container DataSO", menuName = "UI/MVP/Event Container DataSO")]
    public class MVPDataSO : ScriptableObject
    {
        [HideInInspector] public BasePresenter parentPrefab;
        [ReadOnly] public List<FormData> formDataList;
        [HideInInspector] public string modelTypeName;
        [HideInInspector] public string viewTypeName;
        
        #if UNITY_EDITOR
        [HideInInspector] public int selectedChildIndex = 0;
        #endif
    }
}
