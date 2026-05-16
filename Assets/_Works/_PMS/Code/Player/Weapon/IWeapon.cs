using UnityEngine;

public interface IWeapon
{
    GameObject WeaponObject { get; }
    bool CanAttack { get; }
    void Attack(float damage);
}
