using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._PMS.Code.Event;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModule : MonoBehaviour, IModule
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float swapAttackDelay = 0.5f;
    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private RangedWeapon rangedWeapon;

    [SerializeField] private EventChannelSO _playerEventChannel;
    [SerializeField] private EventChannelSO _soundChannel;

    [SerializeField] private GameObject weaponsRoot;

    private List<IWeapon> _weapons = new();
    private int _currentIndex = 0;
    private float _swapDelayTimer = 0f;

    public IWeapon CurrentWeapon => _weapons.Count > 0 ? _weapons[_currentIndex] : null;
    public bool CanAttack => _swapDelayTimer <= 0f && CurrentWeapon != null && CurrentWeapon.CanAttack;

    public void Initialize(ModuleOwner owner)
    {
        if (meleeWeapon != null)
        {
            _weapons.Add(meleeWeapon);
            meleeWeapon.WeaponObject.SetActive(true);
            meleeWeapon.Initialize(_soundChannel);
        }

        if (rangedWeapon != null)
        {
            rangedWeapon.WeaponObject.SetActive(false);
            rangedWeapon.Initialize(this, _playerEventChannel, _soundChannel);
        }
    }

    private void Update()
    {
        if (_swapDelayTimer > 0f)
            _swapDelayTimer -= Time.deltaTime;
    }

    public void Attack()
    {
        if (!CanAttack) return;
        CurrentWeapon.Attack(damage);
    }

    public void SwapNext()
    {
        if (_weapons.Count <= 1) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = (_currentIndex + 1) % _weapons.Count;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();
        _swapDelayTimer = swapAttackDelay;
        _playerEventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(CurrentWeapon is RangedWeapon));
    }

    public void SwapPrev()
    {
        if (_weapons.Count <= 1) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = (_currentIndex - 1 + _weapons.Count) % _weapons.Count;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();
        _swapDelayTimer = swapAttackDelay;
        _playerEventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(CurrentWeapon is RangedWeapon));
    }

    public void SwapWeaponIndex(int index)
    {
        if (_weapons.Count <= index) return;
        if (_weapons[index] == CurrentWeapon) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = index;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();
        _swapDelayTimer = swapAttackDelay;
        _playerEventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(CurrentWeapon is RangedWeapon));
    }

    [ContextMenu("aaaaa")]
    public void AddGun()
    {
        if (rangedWeapon == null) return;

        if (_weapons.Contains(rangedWeapon))
        {
            rangedWeapon.Reroad();
            _playerEventChannel.RaiseEvent(PlayerEvents.BulletChangeEvent.Init(5));
            return;
        }

        CurrentWeapon.WeaponObject.SetActive(false);
        _weapons.Add(rangedWeapon);
        _currentIndex = _weapons.Count - 1;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();
        _swapDelayTimer = swapAttackDelay;
        _playerEventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(true));
        _playerEventChannel.RaiseEvent(PlayerEvents.BulletChangeEvent.Init(5));
    }

    public void ResetWeaponState()
    {
        if (CurrentWeapon is MeleeWeapon melee)
        {
            melee.ForceReset();
        }
        else if (CurrentWeapon is RangedWeapon ranged)
        {
            ranged.ForceReset();
        }
        weaponsRoot.SetActive(false);
    }

    public void RestoreCurrentWeapon()
    {
        weaponsRoot.SetActive(true);
        for (int i = 0; i < _weapons.Count; i++)
            _weapons[i].WeaponObject.SetActive(i == _currentIndex);
    }
}