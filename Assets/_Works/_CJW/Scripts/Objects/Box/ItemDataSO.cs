using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Box
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData", order = 0)]
    public class ItemDataSO : ScriptableObject
    {
        [Header("드랍 확률은 0~100 사이로 입력")]
        public float dropPercent;
        public GameObject dropPrefab;
    }
}