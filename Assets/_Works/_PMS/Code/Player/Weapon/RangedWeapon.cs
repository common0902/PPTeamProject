using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;
using static UnityEngine.UI.Image;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float range = 50f;
    [SerializeField] private LayerMask targetLayer;

    private int _bullets = 5;

    public GameObject WeaponObject => gameObject;
    public bool CanAttack => _bullets > 0;
    public int Bullets => _bullets;

    public void Attack(float damage)
    {
        if (!CanAttack) return;

        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;


        _bullets--;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(damage, direction, origin); 
        }
    }

    public void Reroad() => _bullets = 5;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * range);
    }

}