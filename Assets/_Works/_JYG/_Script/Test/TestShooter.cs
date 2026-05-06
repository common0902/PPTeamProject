using System;
using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JYG._Script.Test
{
    public class TestShooter : MonoBehaviour
    {
        [field: SerializeField] public GameObject TargetGameObject { get; private set; }
        [field: SerializeField] public IDamageable Target { get; private set; }

        private void Awake()
        {
            Target = TargetGameObject.GetComponent<IDamageable>();
        }

        private void Update()
        {
            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                Target.TakeDamage(1f, transform.forward, transform.position);
            }
        }
    }
}
