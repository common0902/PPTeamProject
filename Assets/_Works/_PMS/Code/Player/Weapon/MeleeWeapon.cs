using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask targetLayer;

    public GameObject WeaponObject => gameObject;
    public bool CanAttack => true; // 근접은 항상 공격 가능

    public void Attack(float damage)
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.SphereCast(origin, attackRadius, direction, out RaycastHit hit, attackRange, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(damage, direction, origin);
        }
    }
}