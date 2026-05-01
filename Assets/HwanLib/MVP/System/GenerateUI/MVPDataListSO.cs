using UnityEngine;

namespace HwanLib.MVP.System.GenerateUI
{
    [CreateAssetMenu(fileName = "MVPDataListSO", menuName = "UI/MVP/MVPDataListSO", order = 0)]
    public class MVPDataListSO : ScriptableObject
    {
        public MVPDataSO[] mvpData;
    }
}