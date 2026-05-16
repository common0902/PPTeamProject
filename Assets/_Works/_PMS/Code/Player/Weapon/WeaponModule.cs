using _Script.Agent.Modules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponModule : MonoBehaviour, IModule
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float swapAttackDelay = 0.5f;

    private List<IWeapon> _weapons = new();

    private int _currentIndex = 0;
    private float _swapDelayTimer = 0f;
    private ModuleOwner _owner;

    public IWeapon CurrentWeapon => _weapons.Count > 0 ? _weapons[_currentIndex] : null;
    public bool CanAttack => _swapDelayTimer <= 0f && CurrentWeapon != null && CurrentWeapon.CanAttack;

    public void Initialize(ModuleOwner owner)
    {
        _owner = owner;

        // 비활성화된 게임 오브젝트도 포함해서 찾기
        var found = owner.GetComponentsInChildren<IWeapon>(true);
        foreach (var weapon in found)
            _weapons.Add(weapon);

        // 처음엔 칼만 활성화
        for (int i = 0; i < _weapons.Count; i++)
            _weapons[i].WeaponObject.SetActive(i == 0);
    }

    private void Update()
    {
        if (_swapDelayTimer > 0f)
            _swapDelayTimer -= Time.deltaTime;
    }

    public void Attack()
    {
        if (!CanAttack) return;
        Debug.Log(11111111111111);
        CurrentWeapon.Attack(damage);

        // 탄알 0이면 총 제거
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

    public void AddGun()
    {
        var ranged = _weapons.OfType<RangedWeapon>().FirstOrDefault();
        if (ranged != null)
        {
            ranged.Reroad(); // 이미 있으면 탄알만 충전
            return;
        }

        // 비활성화된 Gun 찾아서 추가
        var gun = _owner.GetComponentsInChildren<RangedWeapon>(true).FirstOrDefault();
        if (gun != null)
        {
            _weapons.Add(gun);
            gun.WeaponObject.SetActive(false); // 스왑 전까지 비활성화 유지
        }
    }

    private void RemoveCurrentWeapon()
    {
        CurrentWeapon.WeaponObject.SetActive(false);
        _weapons.RemoveAt(_currentIndex);
        _currentIndex = Mathf.Clamp(_currentIndex, 0, _weapons.Count - 1);
    }
}