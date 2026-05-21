using System;
using System.Collections.Generic;
using System.Linq;
using _Script.ScriptableObject.Event;
using GameLib.SoundSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace _Works._JTH.Scripts.UI
{
    [Serializable]
    public class FormSoundModule<T> where T : Enum, IConvertible
    {
        [SerializeField] private SoundForm[] soundForms;
        [SerializeField] private EventChannelSO soundChannel;
        
        public void PlaySound(int childIndex)
        {
            // 설정한 것들 중 같은 것ㅇ르
            if (soundForms.FirstOrDefault(form
                    => Convert.ToInt32(form.TargetForm) == childIndex) is var soundForm and not null)
            {
                soundChannel.RaiseEvent(
                    SoundSystemEvents.PlaySoundEvent.Init(Vector3.zero, soundForm.ClipSO));
            }
        }

        [Serializable]
        private class SoundForm
        {
            public T TargetForm;
            public SoundClipSO ClipSO;
        }
    }
}