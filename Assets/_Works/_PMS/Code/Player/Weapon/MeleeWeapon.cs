using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask wallLayer;

    private Animator _animator;
    private int _comboCount = 0;
    private bool _canCombo = false;
    private bool _canAttack = true;

    private static readonly int Attack1Hash = Animator.StringToHash("ATTACK 1");
    private static readonly int Attack2Hash = Animator.StringToHash("ATTACK 2");
    private static readonly int Attack3Hash = Animator.StringToHash("ATTACK 3");
    private static readonly int SwapHash = Animator.StringToHash("SWAP");
    private static readonly int IdleHash = Animator.StringToHash("IDLE");

    public GameObject WeaponObject => gameObject;
    public bool CanAttack => _canAttack;

    private void Awake() => _animator = GetComponent<Animator>();

    public void Attack(float damage)
    {
        if (!_canAttack) return;
        _canAttack = false;
        _canCombo = false;
        ExecuteAttack(damage);
        PlayAttackAnimation();
    }

    private void ExecuteAttack(float damage)
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        float range = attackRange;
        if (Physics.Raycast(origin, direction, out RaycastHit wallHit, attackRange, wallLayer))
            range = wallHit.distance;

        if (Physics.SphereCast(origin, attackRadius, direction, out RaycastHit hit, range, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(damage, direction, origin);
        }
    }

    private void PlayAttackAnimation()
    {
        int hash = _comboCount switch
        {
            0 => Attack1Hash,
            1 => Attack2Hash,
            2 => Attack3Hash,
            _ => Attack1Hash
        };
        _animator.CrossFade(hash, 0.05f);
        _comboCount = (_comboCount + 1) % 3;
    }

    public void CanCombo()
    {
        _canCombo = true;
        _canAttack = true;
    }

    public void CanNotCombo()
    {
        _canCombo = false;
        if (_canAttack)
        {
            _comboCount = 0;
            _animator.CrossFade(IdleHash, 0.1f);
        }
    }

    public void OnSwap()
    {
        _canAttack = true;
        _canCombo = false;
        _comboCount = 0;
        _animator.Play(SwapHash);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up + transform.forward * attackRange, attackRadius);
    }
}