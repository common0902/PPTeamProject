using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.InteractableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class Sabotage : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SabotageDataSo sabotageData; // 사보타지 데이터. 이걸로 어떤 사보타지인지 구별 가능
        [SerializeField] private EventChannelSO sabotageEvent;
        [SerializeField] private EventChannelSO interactEvent; // 상호작용을 해야 작동할 때 필요한 이벤트
        [SerializeField] private Color defaultOutLineColor;
        [SerializeField] private Color interactedOutLineColor;
        [SerializeField] private GameObject visualObject;
        
        [SerializeField] public string targetEventName;
        [SerializeField] private  bool isLocked = false; // 사보타지가 잠금 해제되었는지 여부
        private AbstractSabotageEvent _targetEvent;
        private Outline _outline;
        private Rigidbody _rigid;
        private bool _isUsed = false;

        
        private void Awake()
        {
            sabotageEvent.AddListener<TopViewEvent>(HandleOpen);
            _rigid = GetComponent<Rigidbody>();
            _outline = GetComponentInChildren<Outline>();

            Debug.Log(_outline);
            _outline.OutlineColor = defaultOutLineColor;
            visualObject.SetActive(false);
        }

        private void Start()
        {
            Debug.Log(targetEventName);
            _targetEvent = typeof(SabotageEvents).GetField(targetEventName,
                BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as AbstractSabotageEvent;
            
            interactEvent.AddListener<UnlockEvent>(HandleUnlock);
        }

        private void HandleUnlock(UnlockEvent evt)
        {
            if(isLocked == false
               && evt.TargetSabotageData != null 
               && evt.TargetSabotageData == sabotageData)
            {
                isLocked = true;
                Debug.Log($"{targetEventName} 사보타지 해금");
            }
        }


        private void HandleOpen(TopViewEvent evt)
        {
            if (evt.IsTopView && isLocked)
            {
                visualObject.SetActive(true);
                return;
            }
            if (evt.IsTopView && !isLocked)
            {
                Debug.Log("사용할 수 없음");
            }
            else
            {
                visualObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isUsed || !isLocked) return; 
    
            _outline.enabled = false;
            sabotageEvent.RaiseEvent(_targetEvent.Init(true));
            _isUsed = true;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Enter");
            if(_isUsed) return;

            _outline.OutlineColor = interactedOutLineColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _outline.OutlineColor = defaultOutLineColor;
        }
        
        private void OnDestroy()
        {
            sabotageEvent.RemoveListener<TopViewEvent>(HandleOpen);
            interactEvent.RemoveListener<UnlockEvent>(HandleUnlock);
        }
    }
}
