using System;
using System.Collections;
using System.Threading.Tasks;
using GameLib.PoolObject.Runtime;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace GameLib.SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup musicGroup;
        
        private AudioSource _audioSource;
        public GameObject GameObject => this == null ? null : gameObject;
        [field:SerializeField] public PoolItemSO PoolItem { get; set; }

        public event Action<SoundPlayer> OnSoundFinished;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(SoundClipSO clipData)
        {
            if (clipData.audioType == AudioTypes.Sfx)
            {
                _audioSource.outputAudioMixerGroup = sfxGroup;
            }
            if (clipData.audioType == AudioTypes.Music)
            {
                _audioSource.outputAudioMixerGroup = musicGroup;
            }

            _audioSource.volume = clipData.volume;
            _audioSource.pitch = clipData.pitch;

            if (clipData.randomizePitch)
            {
                _audioSource.pitch += Random.Range(-clipData.randomPitchModifier, clipData.randomPitchModifier);
            }
            
            _audioSource.clip = clipData.audioClip;
            _audioSource.loop = clipData.isLoop;

            if (!clipData.isLoop)
            {
                float duration = _audioSource.clip.length + 2f;
                StartCoroutine(DisableSoundTimer(duration));
                //심화반에서는 아래의 코드를 사용했다.
                // _ = DisableSoundTimer(time);
            }

            _audioSource.Play();
        }

        private IEnumerator DisableSoundTimer(float duration)
        {
            yield return new WaitForSeconds(duration);
            OnSoundFinished?.Invoke(this);
        }

        //심화반에서는 아래의 코드를 사용했다.
        /*
        private async Task<string> DisableSoundTimer(float time)
        {
            await Awaitable.WaitForSecondsAsync(time);
            OnSoundFinished?.Invoke(this);
        }
        */

        public void ForceStopSound()
        {
            _audioSource.Stop();
        }

        public void ResetItem()
        {
            //아무것도 하지 않음.
        }
    }
}