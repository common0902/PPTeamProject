using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy.CombatSystem;
using _Works._PMS.Code.Event;
using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float range = 50f;
    [SerializeField] private LayerMask targetLayer;

    private WeaponModule _weaponModule;
    private EventChannelSO _eventChannel;
    private Animator _animator;
    private bool _canAttack = true;
    private int _bullets = 5;

    private static readonly int AttackHash = Animator.StringToHash("ATTACK");
    private static readonly int SwapHash = Animator.StringToHash("SWAP");
    private static readonly int IdleHash = Animator.StringToHash("IDLE");

    public GameObject WeaponObject => gameObject;
    public bool CanAttack => _canAttack && _bullets > 0;
    public int Bullets => _bullets;

    private void Awake() => _animator = GetComponent<Animator>();

    public void Initialize(WeaponModule weaponModule, EventChannelSO eventChannel)
    {
        _weaponModule = weaponModule;
        _eventChannel = eventChannel;
    }

    public void Attack(float damage)
    {
        if (!CanAttack) return;
        _canAttack = false;
        _bullets--;
        _animator.CrossFade(AttackHash, 0.05f);
        _eventChannel.RaiseEvent(PlayerEvents.BulletChangeEvent.Init(_bullets));

        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(damage, direction, origin);
        }
    }

    public void OnAttackEnd()
    {
        _canAttack = true;
        _animator.CrossFade(IdleHash, 0.1f);
    }

    public void OnSwap()
    {
        _canAttack = true;
        _animator.Play(SwapHash);
    }

    public void Reroad() => _bullets = 5;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 0.05f);
    }
}