using System;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.CombatSystem
{
    public class MeleeEnemyAttackModule : AbstractAttackModule
    {
        [SerializeField] private Vector3 overlapOffset = Vector3.zero;
        [SerializeField] private float radius = 2f;
        [SerializeField] private LayerMask targetLayer;

        private Collider[] player = new Collider[1];


        protected override void HandleAgentAttack()
        {
            base.HandleAgentAttack();
            
            Ray ray = new Ray(transform.position + overlapOffset, transform.forward);
            Physics.OverlapSphereNonAlloc(transform.position + overlapOffset, 0.2f, player, targetLayer);
            if (player[0] != null)
            {
                if (player[0].TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.TakeDamage(0, transform.forward, transform.position);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + overlapOffset, radius);
        }
    }
}
