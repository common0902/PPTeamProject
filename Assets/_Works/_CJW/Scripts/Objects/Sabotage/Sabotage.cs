using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.InteractableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class Sabotage : ModuleOwner, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Sabotage Data")]
        [field: SerializeField]
        public SabotageDataSo SabotageData { get; private set; } // 사보타지 데이터. 이걸로 어떤 사보타지인지 구별 가능

        [Header("Event Channel")]
        [SerializeField] private EventChannelSO cameraEvent;
        [SerializeField] private EventChannelSO sabotageEvent;
        [SerializeField] private EventChannelSO interactEvent; // 상호작용을 해야 작동할 때 필요한 이벤트
        [Header("Info")]

        
        [SerializeField] public string targetEventName;
        [SerializeField] private  bool isLocked = false; // 사보타지가 잠금 해제되었는지 여부
        private AbstractSabotageEvent _targetEvent;
        private bool _isUsed = false;
        private SabotageVisual _visual;
        
        protected override void Awake()
        {
            base.Awake();   
            cameraEvent.AddListener<TopViewEvent>(HandleOpen);
            interactEvent.AddListener<UnlockEvent>(HandleUnlock);
            _visual = GetModule<SabotageVisual>();
        }

        private void Start()
        {
            _targetEvent = typeof(SabotageEvents).GetField(targetEventName,
                BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as AbstractSabotageEvent;
            
        }

        private void HandleUnlock(UnlockEvent evt)
        {
            if(isLocked == true
               && evt.TargetSabotageData != null 
               && evt.TargetSabotageData == SabotageData)
            {
                isLocked = false;
                interactEvent.RemoveListener<UnlockEvent>(HandleUnlock);
                
                Debug.Log($"{targetEventName} 사보타지 해금");
            }
        }


        private void HandleOpen(TopViewEvent evt)
        {
            if (evt.IsTopView && !isLocked)
            {
                _visual.HandleActivation(true, false);
                return;
            }
            if (evt.IsTopView && isLocked)
            {
                Debug.Log("사용할 수 없음");
                _visual.HandleActivation(false, true);
            }
            else
            {
                _visual.HandleActivation(false, false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isUsed || !isLocked) return; 
    
            _visual.HandleOutLineEnable(false);
            sabotageEvent.RaiseEvent(_targetEvent.Init(true));
            _isUsed = true;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_isUsed) return;
            Debug.Log("Enter");
            
            cameraEvent.RaiseEvent(new FocusedSabotageEvent().Init(this, true));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cameraEvent.RaiseEvent(new FocusedSabotageEvent().Init(this, false));;
        }
        
        private void OnDestroy()
        {
            cameraEvent.RemoveListener<TopViewEvent>(HandleOpen);
            interactEvent.RemoveListener<UnlockEvent>(HandleUnlock);
        }
    }
}
