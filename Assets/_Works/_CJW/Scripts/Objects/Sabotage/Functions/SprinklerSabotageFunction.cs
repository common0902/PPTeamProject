using DG.Tweening;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class SprinklerSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject puddleObject;
        [SerializeField] private float duration;
        public override void UseFunction()
        {
            base.UseFunction();
            foreach (ParticleSystem vfx in vfXes)
            {
                var main = vfx.main;
                main.duration = duration;
            }

            var puddle = Instantiate(puddleObject, new Vector3(transform.position.x,1, transform.position.z), Quaternion.identity);
            puddle.SetLifetime(duration, () =>
                soundEventChannel.RaiseEvent(SoundSystemEvents.StopSoundEvent.Init(channelNumber)));
            PlayParticle();
        }
    }
}