using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class AbstractSabotage : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        
        [SerializeField] private EventChannelSO sabotageEvent;
        [SerializeField] private EventChannelSO interactEvent; // 상호작용을 해야 작동할 때 필요한 이벤트 -> 나중에 추상으로 분리할거임
        [SerializeField] private Color defualtOutLineColor;
        [SerializeField] private Color interactedOutLineColor;
        
        [SerializeField] public string targetEventName;
        private GameEvent _targetEvent;
        private Outline _outline;
        private Rigidbody _rigid;
        private bool _isCanOpen;
        private bool _isUsed;

        
        private void Awake()
        {
            sabotageEvent.AddListener<TopViewEvent>(HandleOpen);
            _rigid = GetComponent<Rigidbody>();
            _outline = GetComponent<Outline>();
            // _targetEvent = typeof(SabotageEvents).GetField(_targetEventName, 
            //     BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as GameEvent;

            _outline.OutlineColor = defualtOutLineColor;
        }

        private void Start()
        {
            Debug.Log(targetEventName);
            _targetEvent = typeof(SabotageEvents).GetField(targetEventName,
                BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as GameEvent;
            
        }


        private void HandleOpen(TopViewEvent evt)
        {
            if (evt.IsTopView && _isCanOpen)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);   
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isUsed) return;
            _outline.enabled = false;
            sabotageEvent.RaiseEvent(_targetEvent);
            _isUsed = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_isUsed) return;
            
            _outline.OutlineColor = interactedOutLineColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _outline.OutlineColor = defualtOutLineColor;
        }
    }
}
