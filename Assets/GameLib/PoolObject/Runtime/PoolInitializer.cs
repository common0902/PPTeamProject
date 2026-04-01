using UnityEngine;

namespace GameLib.PoolObject.Runtime
{
    public class PoolInitializer : MonoBehaviour
    {
        [field: SerializeField] public PoolManagerSO PoolManager { get; private set; }

        private void Awake()
        {
            PoolManager.InitializePool(transform);

            PoolInitializer[] initializers = FindObjectsByType<PoolInitializer>(FindObjectsSortMode.None);
            Debug.Assert(initializers.Length == 1, "풀 매니저가 2개 이상입니다!!");
        }
        
        public T Pop<T>(PoolItemSO type) where T : IPoolable => PoolManager.Pop<T>(type);
        public void Push(IPoolable item) => PoolManager.Push(item);
    }
}