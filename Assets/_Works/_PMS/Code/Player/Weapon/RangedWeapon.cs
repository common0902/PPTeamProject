using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy.CombatSystem;
using _Works._PMS.Code.Event;
using GameLib.SoundSystem;
using UnityEngine;

public class RangedWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private SoundClipSO attackSound;
    [SerializeField] private SoundClipSO swapSound;

    [SerializeField] private float range = 50f;
    [SerializeField] private LayerMask targetLayer;

    private WeaponModule _weaponModule;

    private EventChannelSO _playerEventChannel;
    private EventChannelSO _soundChannel;

    private Animator _animator;
    private bool _canAttack = true;
    private int _bullets = 5;

    private float _damage;

    private static readonly int AttackHash = Animator.StringToHash("ATTACK");
    private static readonly int SwapHash = Animator.StringToHash("SWAP");
    private static readonly int IdleHash = Animator.StringToHash("IDLE");
    private Camera _mainCam;

    public GameObject WeaponObject => gameObject;
    public bool CanAttack => _canAttack && _bullets > 0;
    public int Bullets => _bullets;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Initialize(WeaponModule weaponModule, EventChannelSO playerEventChannel, EventChannelSO soundChannel)
    {
        _weaponModule = weaponModule;
        _playerEventChannel = playerEventChannel;
        _soundChannel = soundChannel;
        _mainCam = Camera.main;
    }

    public void Attack(float damage)
    {
        if (!CanAttack)
        {
            if (_bullets <= 0)
                _playerEventChannel.RaiseEvent(PlayerEvents.BulletShortageEvent);
            return;
        }
        _canAttack = false;
        _bullets--;
        _damage = damage;
        PlaySound(attackSound);
        _animator.CrossFade(AttackHash, 0.05f);
        _playerEventChannel.RaiseEvent(PlayerEvents.BulletChangeEvent.Init(_bullets));
        
    }

    public void OnAttackHit()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = _mainCam.gameObject.transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage, direction, origin);
        }
    }

    public void OnAttackEnd()
    {
        _canAttack = true;
        _animator.Play(IdleHash);
    }
    public void OnSwap()
    {
        _canAttack = true;
        _animator.Play(SwapHash);
        PlaySound(swapSound);
    }

    public void Reroad() => _bullets = 5;

    private void PlaySound(SoundClipSO sound)
    {
        if (sound == null || _soundChannel == null) return;
        _soundChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, sound));
    }

    public void ForceReset()
    {
        _canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position + Vector3.up, _mainCam.gameObject.transform.forward * range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 0.05f);
    }
}