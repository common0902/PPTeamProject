using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy.CombatSystem;
using GameLib.SoundSystem;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private SoundClipSO attack1Sound;
    [SerializeField] private SoundClipSO attack2Sound;
    [SerializeField] private SoundClipSO attack3Sound;
    [SerializeField] private SoundClipSO swapSound;
    private EventChannelSO _soundChannel;

    private Animator _animator;
    private int _comboCount = 0;
    private bool _canCombo = false;
    private bool _canAttack = true;

    private float _pendingDamage;

    private static readonly int Attack1Hash = Animator.StringToHash("ATTACK 1");
    private static readonly int Attack2Hash = Animator.StringToHash("ATTACK 2");
    private static readonly int Attack3Hash = Animator.StringToHash("ATTACK 3");
    private static readonly int SwapHash = Animator.StringToHash("SWAP");
    private static readonly int IdleHash = Animator.StringToHash("IDLE");

    public GameObject WeaponObject => gameObject;
    public bool CanAttack => _canAttack;

    public void Initialize(EventChannelSO soundChannel)
    {
        _soundChannel = soundChannel;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack(float damage)
    {
        if (!_canAttack) return;
        _canAttack = false;
        _canCombo = false;
        _pendingDamage = damage;
        PlayAttackAnimation();
    }
    public void OnAttackHit()
    {
        ExecuteAttack(_pendingDamage);
    }

    private void ExecuteAttack(float damage)
    {
        Vector3 origin = transform.position + Vector3.up - transform.forward * attackRadius * 2;
        Vector3 direction = transform.forward;

        float range = attackRange + attackRadius;
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
        int hash;
        SoundClipSO sound;

        switch (_comboCount)
        {
            case 0: hash = Attack1Hash; sound = attack1Sound; break;
            case 1: hash = Attack2Hash; sound = attack2Sound; break;
            case 2: hash = Attack3Hash; sound = attack3Sound; break;
            default: hash = Attack1Hash; sound = attack1Sound; break;
        }

        _animator.CrossFade(hash, 0.05f);
        PlaySound(sound);
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
        PlaySound(swapSound);
    }

    private void PlaySound(SoundClipSO sound)
    {
        if (sound == null || _soundChannel == null) return;
        _soundChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, sound));
    }

    public void ForceReset()
    {
        _canAttack = true;
        _canCombo = false;
        _comboCount = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up - transform.forward * attackRadius * 2;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin + transform.forward * (attackRange + attackRadius), attackRadius);
    }
}