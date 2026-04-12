using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MVP.System
{
    [CreateAssetMenu(fileName = "Event Container DataSO", menuName = "UI/MVP/Event Container DataSO")]
    public class MVPDataSO : ScriptableObject
    {
        [HideInInspector] public BasePresenter parentPrefab;
        public List<FormData> formDataList;
        [HideInInspector] public string modelTypeName;
        [HideInInspector] public string viewTypeName;
        
        #if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (formDataList != backupFormDataList)
            {
                Debug.LogWarning("리스트는 직접 수정할 수 없습니다.");
                formDataList = backupFormDataList;
            }
        }
        
        [HideInInspector] public List<FormData> backupFormDataList;
        [HideInInspector] public int selectedChildIndex = 0;
        #endif
    }
}
