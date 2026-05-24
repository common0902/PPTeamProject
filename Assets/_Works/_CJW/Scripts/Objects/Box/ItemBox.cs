using System;
using System.Collections.Generic;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Helpers;
using _Works._JYG._Script.Enemy.CombatSystem;
using GameLib.SoundSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Works._CJW.Scripts.Objects.Box
{
    public class ItemBox : MonoBehaviour, IDamageable
    {
        [SerializeField] private SoundClipSO soundData;
        [SerializeField] private EventChannelSO soundChannel;
        [Header("확률: 0 ~ 100")]
        [SerializeField] private List<ItemDataSO> itemDatas;
        private ItemGatcha _gatcha;

        private void Awake()
        {
            _gatcha = new ItemGatcha(itemDatas);
        }
        public void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition)
        {
            if (damage > 0)
            {
                Drop();
            }
        }

        private void Drop()
        {
            ItemDataSO droppedItem = _gatcha.GetRandomItem();
            if (droppedItem != null)
            {
                soundChannel.RaiseEvent
                    (SoundSystemEvents.PlaySoundEvent.Init(transform.position, soundData));
                Instantiate(droppedItem.dropPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            // 랜덤 수 뽑고 그게 확률보다 작으면 
        }
    }
}
