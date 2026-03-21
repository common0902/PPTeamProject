using UnityEngine;

namespace _Script.ScriptableObject.Datas.Table
{
    public class AbstractDataTableSO : UnityEngine.ScriptableObject
    {
        [field:SerializeField] public string TableName { get; private set; }
        [field: SerializeField] public IndexSO[] AssetList { get; private set; }
    }
}