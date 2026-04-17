using System;
using System.Collections.Generic;
using UnityEngine;

namespace HwanLib.MVP.System
{
    [CreateAssetMenu(fileName = "MVPDataSO", menuName = "UI/MVP/MVPDataSO")]
    public class MVPDataSO : ScriptableObject
    {
        [HideInInspector] public BasePresenter parentPrefab; 
        [ReadOnly] [SerializeField] private FormDataList formDataList;

        // Type은 직렬화가 되지 않기 때문에 저장이 안됨. 그래서 string으로 직렬화하고 
        [HideInInspector] public string modelTypeName;
        [HideInInspector] public string viewTypeName;
        
        public Type GetViewType() => EditorInfo.GetType(EditorInfo.UIAssembly, viewTypeName);
        public Type GetModelType() => EditorInfo.GetType(EditorInfo.UIAssembly, modelTypeName);
        
        public FormData GetFormData(string key)
        {
            formDataList ??= new FormDataList();
            
            foreach (FormData formData in formDataList.list)
            {
                if (formData.gameObjectName == key)
                    return formData;
            }

            return null;
        }

        public void SetFormDataList(List<FormData> formList)
        {
            formDataList.list = formList;
        }

        public List<string> GetFormDataKeys()
        {
            formDataList ??= new FormDataList();
            
            List<string> keys = new List<string>();

            foreach (FormData formData in formDataList.list)
            {
                keys.Add(formData.gameObjectName);
            }

            return keys;
        }

        public void RemoveFormData(string key)
        {
            formDataList ??= new FormDataList();

            for (int i = 0; i < formDataList.list.Count; i++)
            {
                if (formDataList.list[i].gameObjectName == key)
                    formDataList.list.RemoveAt(i);
            }
        }

        public List<FormData> GetFormDataList()
        { 
            formDataList ??= new FormDataList();
            return formDataList.list;
        }

        public void ResetFormData()
        {
            formDataList = null;
        }
        
        #if UNITY_EDITOR
        [HideInInspector] public string selectedChildName;
        #endif
        
        [Serializable]
        public class FormDataList
        {
            public List<FormData> list;
        }
    }
}
