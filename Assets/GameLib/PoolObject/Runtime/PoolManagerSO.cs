using System.Collections.Generic;
using UnityEngine;

namespace GameLib.PoolObject.Runtime
{
    [CreateAssetMenu(fileName = "Pool Manager", menuName = "Object Pool/PoolManager", order = 0)]
    public class PoolManagerSO : ScriptableObject
    {
        public List<PoolItemSO> itemList = new();
        
        private Dictionary<PoolItemSO, Pool> _pools;
        private Transform _rootTrm;

        public void InitializePool(Transform rootTrm)
        {
            _rootTrm = rootTrm;
            _pools = new Dictionary<PoolItemSO, Pool>();

            foreach (PoolItemSO item in itemList)
            {
                IPoolable poolable = item.prefab.GetComponent<IPoolable>();
                Debug.Assert(poolable != null, $"Poolable 컴포넌트가 존재하지 않습니다! {item.prefab.name}");

                Pool pool = new Pool(item, _rootTrm, item.initCount);
                _pools.Add(item, pool);
            }
        }

        public T Pop<T>(PoolItemSO type) where T : IPoolable
        {
            Debug.Assert(_rootTrm != null, $"오브젝트 풀이 초기화 되어있지 않습니다!");

            if (_pools.TryGetValue(type, out Pool pool))
            {
                return (T)pool.Pop();
            }

            return default;
        }

        public void Push(IPoolable item)
        {
            if (_pools.TryGetValue(item.PoolItem, out Pool pool))
                pool.Push(item);
        }
    }
}