using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using GameLib.PoolObject.Runtime;
using UnityEngine;

namespace GameLib.SoundSystem
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManagerSO;
        [SerializeField] private PoolItemSO soundItemSO;

        [field: SerializeField] public EventChannelSO SoundEventChannel { get; private set; }

        private readonly Dictionary<int, SoundPlayer> _soundPlayerDict = new();
        
        #region EventInit

        private void Awake()
        {
            SoundEventChannel.AddListener<PlaySoundEvent>(HandlePlaySoundEvent);
            SoundEventChannel.AddListener<StopSoundEvent>(HandleStopSoundEvent);
        }

        private void OnDestroy()
        {
            
            SoundEventChannel.RemoveListener<PlaySoundEvent>(HandlePlaySoundEvent);
            SoundEventChannel.RemoveListener<StopSoundEvent>(HandleStopSoundEvent);
        }
        
        #endregion

        private void HandlePlaySoundEvent(PlaySoundEvent evt)
        {
            SoundPlayer soundPlayer = poolManagerSO.Pop<SoundPlayer>(soundItemSO);
            
            soundPlayer.transform.position = evt.Position;
            soundPlayer.PlaySound(evt.ClipData);
            soundPlayer.OnSoundFinished += HandleSoundFinish;

            if (evt.ChannelNumber > 0 && evt.ClipData.isLoop)
            {
                if (_soundPlayerDict.TryGetValue(evt.ChannelNumber, out SoundPlayer beforeSoundPlayer))
                {
                    beforeSoundPlayer.ForceStopSound();
                    poolManagerSO.Push(beforeSoundPlayer);
                    _soundPlayerDict.Remove(evt.ChannelNumber);
                }
                _soundPlayerDict.Add(evt.ChannelNumber, soundPlayer);
            }
            else if (evt.ChannelNumber <= 0 && evt.ClipData.isLoop)
            {
                Debug.LogWarning($"Clip의 Loop가 활성화 된 경우, 올바른 Channel Number가 필요합니다. GameObject {gameObject.name} : Channel Number {evt.ChannelNumber}");
            }
        }

        private void HandleSoundFinish(SoundPlayer soundPlayer)
        {
            soundPlayer.OnSoundFinished -= HandleSoundFinish;
            poolManagerSO.Push(soundPlayer);
        }

        private void HandleStopSoundEvent(StopSoundEvent evt)
        {
            if (_soundPlayerDict.TryGetValue(evt.ChannelNumber, out SoundPlayer beforeSoundPlayer))
            {
                beforeSoundPlayer.ForceStopSound();
                beforeSoundPlayer.OnSoundFinished -= HandleSoundFinish;
                poolManagerSO.Push(beforeSoundPlayer);
                _soundPlayerDict.Remove(evt.ChannelNumber);
            }
        }
    }
}