using UnityEngine;

namespace GameLib.PoolObject.Runtime
{
    public interface IPoolable
    {
        public PoolItemSO PoolItem { get; }
        public GameObject GameObject { get; }
        public void ResetItem();
    }
}
