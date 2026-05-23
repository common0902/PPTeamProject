using System;
using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Works._JYG._Script.Enemy
{
    public class DancingEnemy : MonoBehaviour, IDamageable
    {
        public UnityEvent OnDeath;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition)
        {
            _animator.CrossFade("DANCE", 0.2f);
            OnDeath?.Invoke();
        }
    }
}
