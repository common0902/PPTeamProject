using UnityEngine;

namespace _Script.ScriptableObject
{
    public abstract class IndexSO : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public int Index { get; set; }
    }
}