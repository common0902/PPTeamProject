using System;
using System.Collections;
using _Script.ScriptableObject.Event;
using _Works._JYG._Script.EventChannel.UIEvent;
using UnityEngine;

namespace _Works._JYG._Script.UI
{
    public class EnemyMarkUI : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO IconActiveEventChannel { get; private set; }

        [SerializeField]
        [Range(0.1f, 5f)] private float onOffDuration;

        private SpriteRenderer _spriteRenderer;
        
        private Camera _mainCam;
        public Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam;
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            IconActiveEventChannel.AddListener<IconActiveOff>(HandleIconOffEvent);
            IconActiveEventChannel.AddListener<IconActiveOn>(HandleIconOnEvent);
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
            float currentTime = Time.time;
            
            Color tempColor = _spriteRenderer.color;

            while (percent < 1)
            {
                percent += Time.deltaTime / onOffDuration;
                if (changeToOff)
                    tempColor.a = 1 - percent;
                else
                    tempColor.a = percent;

                _spriteRenderer.color = tempColor;

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
            transform.up = MainCam.transform.up;
            transform.forward = MainCam.transform.forward;
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
