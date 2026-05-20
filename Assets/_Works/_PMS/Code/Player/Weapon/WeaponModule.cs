using _Script.Agent.Modules;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModule : MonoBehaviour, IModule
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float swapAttackDelay = 0.5f;
    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private RangedWeapon rangedWeapon;

    private List<IWeapon> _weapons = new();
    private int _currentIndex = 0;
    private float _swapDelayTimer = 0f;
    private ModuleOwner _owner;

    public IWeapon CurrentWeapon => _weapons.Count > 0 ? _weapons[_currentIndex] : null;
    public bool CanAttack => _swapDelayTimer <= 0f && CurrentWeapon != null && CurrentWeapon.CanAttack;

    public void Initialize(ModuleOwner owner)
    {
        _owner = owner;

        if (meleeWeapon != null)
        {
            _weapons.Add(meleeWeapon);
            meleeWeapon.WeaponObject.SetActive(true);
        }

        if (rangedWeapon != null)
            rangedWeapon.WeaponObject.SetActive(false);
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

        if (CurrentWeapon is RangedWeapon ranged && ranged.Bullets == 0)
            RemoveCurrentWeapon();
    }

    public void SwapNext()
    {
        if (_weapons.Count <= 1) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = (_currentIndex + 1) % _weapons.Count;
        CurrentWeapon.WeaponObject.SetActive(true);
        _swapDelayTimer = swapAttackDelay;
    }

    public void SwapPrev()
    {
        if (_weapons.Count <= 1) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = (_currentIndex - 1 + _weapons.Count) % _weapons.Count;
        CurrentWeapon.WeaponObject.SetActive(true);
        _swapDelayTimer = swapAttackDelay;
    }

    public void SwapWeaponIndex(int index)
    {
        if (_weapons.Count <= index) return;
        if (_weapons[index] == CurrentWeapon) return;
        CurrentWeapon.WeaponObject.SetActive(false);
        _currentIndex = index;
        CurrentWeapon.WeaponObject.SetActive(true);
        _swapDelayTimer = swapAttackDelay;
    }

    [ContextMenu("AddGun")]
    public void AddGun()
    {
        if (rangedWeapon == null) return;

        if (_weapons.Contains(rangedWeapon))
        {
            rangedWeapon.Reroad();
            return;
        }

        _weapons.Add(rangedWeapon);
        rangedWeapon.WeaponObject.SetActive(false);
        SwapWeaponIndex(1);
    }

    private void RemoveCurrentWeapon()
    {
        CurrentWeapon.WeaponObject.SetActive(false);
        _weapons.RemoveAt(_currentIndex);

        if (_weapons.Count == 0) return;

        
        _currentIndex = 0;
        CurrentWeapon.WeaponObject.SetActive(true);
    }
}