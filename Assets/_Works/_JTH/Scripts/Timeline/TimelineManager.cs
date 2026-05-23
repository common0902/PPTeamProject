using System.Collections;
using _Script.ScriptableObject.Event;
using GameLib.SoundSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace _Works._JTH.Scripts.Timeline
{
    public class TimelineManager : MonoBehaviour
    {
        private readonly int _idle = Animator.StringToHash("IDLE");
        private readonly int _dance1 = Animator.StringToHash("DANCE1");
        private readonly int _dance2 = Animator.StringToHash("DANCE2");
        private readonly int _change = Animator.StringToHash("CHANGE");
        
        [SerializeField] private EventChannelSO soundChannel;
        [SerializeField] private SoundClipSO bgmClipSO;
        [SerializeField] private Animator sniperAnimator;
        [SerializeField] private Animator gunnerAnimator;

        private void Start()
        {
            StartCoroutine(WaitForStart());
        }

        private IEnumerator WaitForStart()
        {
            yield return new WaitForSecondsRealtime(1);
            GetComponent<PlayableDirector>().Play();
        }

        public void PlayBGM()
        {
            soundChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(Vector3.zero, bgmClipSO, 1234));
        }
        
        public void StopBGM()
        {
            soundChannel.RaiseEvent(SoundSystemEvents.StopSoundEvent.Init(1234));
        }

        public void Dance1Gunner()
        {
            gunnerAnimator.CrossFadeInFixedTime(_dance1, 0.1f, 0, 0);
        }
        
        public void Dance2Gunner()
        {
            gunnerAnimator.CrossFadeInFixedTime(_dance2, 0.1f, 0, 0);
        }
        
        public void StopGunner()
        {
            gunnerAnimator.CrossFadeInFixedTime(_change, 0.1f, 0, 0);
        }
        
        public void Dance1Sniper()
        {
            sniperAnimator.CrossFadeInFixedTime(_dance1, 0.1f, 0, 0);
        }
        
        public void Dance2Sniper()
        {
            sniperAnimator.CrossFadeInFixedTime(_change, 0.1f, 0, 0);
        }
        
        public void StopSniper()
        {
            sniperAnimator.CrossFadeInFixedTime(_idle, 0.3f, 0, 0);
        }
    }
}
