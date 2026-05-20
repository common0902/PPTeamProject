using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public abstract class AbstractSabotageFunctionModule : MonoBehaviour, IModule, ISabotageFunctionModule
    {
        [SerializeField] protected EventChannelSO soundEventChannel;
        [SerializeField] protected SoundClipSO startSoundClip;
        
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] protected ParticleSystem[] vfXes;
        [SerializeField] protected int channelNumber;
        protected Sabotage _owner;
        public virtual void Initialize(ModuleOwner moduleOwner)
        {
            _owner = moduleOwner as Sabotage;
        }

        public virtual void UseFunction()
        {
            soundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, startSoundClip, channelNumber));
        }

        protected void PlayParticle()
        {
            foreach (ParticleSystem particle in vfXes)
            {
                particle.Play();
            }
        }
        
        protected bool GetGround(out RaycastHit hit)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundLayer))
            {
                return true;
            }

            return false;
        }
    }
}