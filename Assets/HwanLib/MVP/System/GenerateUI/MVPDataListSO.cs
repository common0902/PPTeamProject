using System;
using UnityEngine;

namespace HwanLib.MVP.System.GenerateUI
{
    [CreateAssetMenu(fileName = "MVPDataListSO", menuName = "UI/MVP/MVPDataListSO", order = 0)]
    public class MVPDataListSO : ScriptableObject
    {
        public MVPDataSO[] singleUIData;
        public MultiableUIData[] multiableUIData;

        public MVPDataSO GetDataFromMultiable(Type presenterType)
        {
            Debug.LogWarning("기본 값보다 많은 수의 UI가 필요합니다. 성능 문제가 발생할 수 있습니다.");
            // 웬만하면 호출되지 않는 것이 좋다.
            foreach (MultiableUIData data in multiableUIData)
            {
                if (data.MvpDataSO.parentPrefab.GetType() == presenterType)
                    return data.MvpDataSO;
            }

            return null;
        }
    }

    [Serializable]
    public struct MultiableUIData
    {
        public MVPDataSO MvpDataSO;
        public int Count;
    }
}