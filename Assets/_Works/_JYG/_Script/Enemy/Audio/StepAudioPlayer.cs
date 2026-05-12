using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using GameLib.PoolObject.Runtime;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.Audio
{
    public class StepAudioPlayer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO SoundEventChannel { get; private set; }
        [field: SerializeField] public Transform FootTrm { get; private set; }
        public List<SoundClipSO> stepSoundClips = new List<SoundClipSO>();

        public void PlayRandomStepSoundClip()
        {
            if (stepSoundClips.Count == 0)
            {
                SoundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(FootTrm.position, stepSoundClips[0]));
                return;
            }
            int index = Random.Range(0, stepSoundClips.Count);
            SoundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(FootTrm.position, stepSoundClips[index]));
        }
    }
}
