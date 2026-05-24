using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using GameLib.SoundSystem;
using UnityEngine;

public class ItemCollectModule : MonoBehaviour, IModule
{
    [SerializeField] private SoundClipSO soundData;
    [SerializeField] private EventChannelSO soundChannel;
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
            soundChannel.RaiseEvent( SoundSystemEvents.PlaySoundEvent.Init(transform.position, soundData));
            _health.Heal(healAmount);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("GunItem"))
        {
            soundChannel.RaiseEvent( SoundSystemEvents.PlaySoundEvent.Init(transform.position, soundData));
            _weapon.AddGun();
            Destroy(other.gameObject);
        }
    }
}