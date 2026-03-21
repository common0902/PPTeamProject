using System;
using UnityEngine;

namespace GameLib.PoolObject.Runtime
{
    [CreateAssetMenu(fileName = "Pool Item", menuName = "Object Pool/Pool Item", order = 10)]
    public class PoolItemSO : ScriptableObject
    {
        public string poolingName;
        public GameObject prefab;
        public int initCount;

        private void OnValidate()
        {
            if (prefab != null && !prefab.TryGetComponent<IPoolable>(out IPoolable _))
            {
                Debug.LogError($"오브젝트 prefab : {prefab.name}속 IPoolable이 존재하지 않습니다.");
                prefab = null;
            }
        }
    }
}
