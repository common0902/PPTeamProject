using System;
using System.Collections;
using GameLib.PoolObject.Runtime;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.CombatSystem
{
    public class EnemyBullet : MonoBehaviour, IPoolable
    {
        private PoolManagerSO _poolManager;
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        [SerializeField] private float damage = 1f;
        public GameObject GameObject => gameObject;
    
        private Rigidbody _rigidbody;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _poolManager = GameManager.Instance.PoolInitializer.PoolManager;
        }

        public void ResetItem()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            transform.position = Vector3.zero;
            StartCoroutine(BulletLifeTime());
        }

        private IEnumerator BulletLifeTime()
        {
            yield return new WaitForSeconds(2f);
            Debug.Log($"맵이 유효하지 않거나 뚫려있어 LifeTime에 의해 EnemyBullet({gameObject.name})이 제거되었습니다.");
            _poolManager.Push(this);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage, Vector3.forward);
            }

            Debug.Log("Hit!");
            StopAllCoroutines();
            _poolManager.Push(this);
        }
    }
}
