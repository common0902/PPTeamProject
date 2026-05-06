using System;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.CombatSystem
{
    public class MeleeEnemyAttackModule : MonoBehaviour
    {
        [SerializeField] private Vector3 overlapOffset = Vector3.zero;
        [SerializeField] private float radius = 2f;
        private void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.OverlapSphere(transform.position, 0.2f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            
        }
    }
}
