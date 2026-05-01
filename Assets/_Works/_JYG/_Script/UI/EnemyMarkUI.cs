using System;
using System.Collections;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy;
using _Works._JYG._Script.EventChannel.UIEvent;
using UnityEngine;

namespace _Works._JYG._Script.UI
{
    public class EnemyMarkUI : MonoBehaviour, IModule
    {
        [field: SerializeField] public EventChannelSO IconActiveEventChannel { get; private set; }
        [SerializeField]
        [Range(0.1f, 5f)] private float onOffDuration;
        [field:SerializeField] public Gradient IconColorGradient { get; private set; }
        
        private AbstractEnemy _enemy;
        private SpriteRenderer _spriteRenderer;
        private float _iconAlpha = 1f;
        
        public void Initialize(ModuleOwner moduleOwner)
        {
            _enemy = moduleOwner as AbstractEnemy;
            IconActiveEventChannel.AddListener<IconActiveOff>(HandleIconOffEvent);
            IconActiveEventChannel.AddListener<IconActiveOn>(HandleIconOnEvent);
        }
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
        }

        private void HandleIconOffEvent(IconActiveOff evt)
        {
            StartCoroutine(ChangeAlpha(true));
        }

        private void HandleIconOnEvent(IconActiveOn obj)
        {
            StartCoroutine(ChangeAlpha(false));
        }

        private IEnumerator ChangeAlpha(bool changeToOff) //On에서 Off로 가고싶으면 true 하면 된다.
        {
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime / onOffDuration;
                if (changeToOff)
                    _iconAlpha = 1 - percent;
                else
                    _iconAlpha = percent;

                yield return null;
            }
        }

        private void OnDestroy()
        {
            IconActiveEventChannel.RemoveListener<IconActiveOff>(HandleIconOffEvent);
            IconActiveEventChannel.RemoveListener<IconActiveOn>(HandleIconOnEvent);
        }

        private void LateUpdate()
        {
            Color newColor = IconColorGradient.Evaluate(_enemy.GetEnemyCaution);
            newColor.a = _iconAlpha;
            _spriteRenderer.color = newColor;
        }

        [ContextMenu("OffIcons")]
        public void OffIcons()
        {
            IconActiveEventChannel.RaiseEvent(IconEvents.IconActiveOff);
        }
        
        [ContextMenu("OnIcons")]
        public void OnIcons()
        {
            IconActiveEventChannel.RaiseEvent(IconEvents.IconActiveOn);
        }
    }
}
