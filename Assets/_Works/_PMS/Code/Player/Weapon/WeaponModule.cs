using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._PMS.Code.Event;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;

public class WeaponModule : MonoBehaviour, IModule
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float swapAttackDelay = 0.5f;
    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private RangedWeapon rangedWeapon;

    [SerializeField] private EventChannelSO _eventChannel;

    private List<IWeapon> _weapons = new();
    private int _currentIndex = 0;
    private float _swapDelayTimer = 0f;

    public float DEBUG_SwapTimer => _swapDelayTimer;
    public IWeapon DEBUG_CurrentWeapon => CurrentWeapon;
    public bool DEBUG_WeaponCanAttack => CurrentWeapon?.CanAttack ?? false;

    public IWeapon CurrentWeapon => _weapons.Count > 0 ? _weapons[_currentIndex] : null;
    public bool CanAttack => _swapDelayTimer <= 0f && CurrentWeapon != null && CurrentWeapon.CanAttack;

    public void Initialize(ModuleOwner owner)
    {
        if (meleeWeapon != null)
        {
            _weapons.Add(meleeWeapon);
            meleeWeapon.WeaponObject.SetActive(true);
        }

        if (rangedWeapon != null)
        {
            rangedWeapon.WeaponObject.SetActive(false);
            rangedWeapon.Initialize(this, _eventChannel);
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
    }

    public void SwapPrev()
    {
        if (_weapons.Count <= 1) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = (_currentIndex - 1 + _weapons.Count) % _weapons.Count;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();
        _swapDelayTimer = swapAttackDelay;
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

        bool isGun = CurrentWeapon is RangedWeapon;
        _eventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(isGun));
    }

    [ContextMenu("111")]
    public void AddGun()
    {
        if (rangedWeapon == null) return;

        if (_weapons.Contains(rangedWeapon))
        {
            rangedWeapon.Reroad();
            return;
        }

        CurrentWeapon.WeaponObject.SetActive(false);
        _weapons.Add(rangedWeapon);
        _currentIndex = _weapons.Count - 1;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();
        _swapDelayTimer = swapAttackDelay;

        _eventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(true));
    }

    public void RemoveCurrentWeapon()
    {
        CurrentWeapon.WeaponObject.SetActive(false);
        _weapons.RemoveAt(_currentIndex);

        if (_weapons.Count == 0) return;

        _currentIndex = 0;
        CurrentWeapon.WeaponObject.SetActive(true);
        CurrentWeapon.OnSwap();

        _eventChannel.RaiseEvent(PlayerEvents.WeaponChangeEvent.Init(false));
    }
}