using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class GasSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject gasObject;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private float duration;

        public override void UseFunction()
        {
            base.UseFunction();
            Instantiate(gasObject, spawnPos.position, Quaternion.identity);
            
            var puddle = Instantiate(gasObject, transform.position, Quaternion.identity);
            puddle.SetLifetime(duration, () =>
                soundEventChannel.RaiseEvent(SoundSystemEvents.StopSoundEvent.Init(channelNumber)));
        }
    }
}