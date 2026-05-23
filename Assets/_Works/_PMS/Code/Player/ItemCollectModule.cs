using _Script.Agent.Modules;
using UnityEngine;

public class ItemCollectModule : MonoBehaviour, IModule
{
    [SerializeField] private float healAmount = 100f;

    private HealthModule _health;
    private WeaponModule _weapon;

    public void Initialize(ModuleOwner owner)
    {
        _health = owner.GetModule<HealthModule>();
        _weapon = owner.GetModule<WeaponModule>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealItem"))
        {
            _health.Heal(healAmount);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("GunItem"))
        {
            _weapon.AddGun();
            Destroy(other.gameObject);
        }
    }
}