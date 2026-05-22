using System;
using System.Diagnostics.Tracing;
using System.Reflection;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.InteractableObjects;
using _Works._CJW.Scripts.Objects.Sabotage.Functions;
using GameLib.SoundSystem;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class Sabotage : ModuleOwner, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // [SerializeField] private EventChannelSO soundEventChannel;
        // [SerializeField] private SoundClipSO soundClipData;
        
        [Header("Sabotage Data")]
        [field: SerializeField] public SabotageDataSo SabotageData { get; private set; } // 사보타지 데이터. 이걸로 어떤 사보타지인지 구별 가능

        [Header("Event Channel")]
        [SerializeField] private EventChannelSO cameraEvent;
        [SerializeField] private EventChannelSO sabotageEvent;
        [SerializeField] private EventChannelSO interactEvent; // 상호작용을 해야 작동할 때 필요한 이벤트
        [Header("Target Event")]
        [SerializeField] public string targetEventName;
        [Header("Mark Offset")]
        [SerializeField] public Vector3 markOffset; // 위치가 안맞을 수 있어서 오프셋 추가
        [SerializeField] public Vector2 markBoxSize; // 마크의 상단을 맞춰줄 박스

        [Header("Together Sabotages")]
        [SerializeField] private Sabotage[] sabotages;
        
        [field: SerializeField] public bool IsLocked { get; private set; } = false; // 사보타지가 잠금 해제되었는지 여부
                
        public bool ShouldMark { get; private set; } = true; // 마킹해야하는지
        public bool IsUsed { get; private set; } = false;

        private bool _isTopView;
        private AbstractSabotageEvent _targetEvent;
        private SabotageVisual _visual;
        private ISabotageFunctionModule _functionModule;
        
        protected override void Awake()
        {
            base.Awake();   
            cameraEvent.AddListener<TopViewEvent>(HandleOpen);
            _visual = GetModule<SabotageVisual>();
            _functionModule = GetModule<ISabotageFunctionModule>();
        }

        private void Start()
        {
            _targetEvent = typeof(SabotageEvents).GetField(targetEventName,
                BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as AbstractSabotageEvent;
            cameraEvent.RaiseEvent(new RegisterSabotageEvent().Init(this, true));
            _visual.HandleActivation(true, false);
            _visual.HandleOutLineEnable(false);
        }

        public void UnlockSabotage()
        {
            IsLocked = false;
        }        
        public void lockSabotage()
        {
            IsLocked = true;
        }

        private void HandleOpen(TopViewEvent evt)
        {
            _isTopView = evt.IsTopView;
            
            if (_isTopView && !IsLocked)
            {
                _visual.HandleOutLineEnable(true);
                _visual.HandleActivation(true, false);
                return;
            }
            if ((_isTopView && IsLocked) || IsUsed)
            {
                Debug.Log("사용할 수 없음");
                _visual.HandleOutLineEnable(false);
                _visual.HandleActivation(false, true);
            }
            else
            {
                _visual.HandleActivation(false, false);
                _visual.HandleOutLineEnable(false);
            }
        }

        public void ActiveVisual(bool unlockVisual, bool lockVisual)
            => _visual.HandleActivation(unlockVisual, lockVisual);

        public void UseFunction()
        {
            if(IsUsed || IsLocked) return; 
    
            _visual.HandleOutLineEnable(false);
            _visual.HandleActivation(false, true);
            _functionModule.UseFunction();
            sabotageEvent.RaiseEvent(_targetEvent.Init(true));
            IsUsed = true;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(IsUsed || IsLocked) return;

            UseFunction();
            foreach (var sabotage in sabotages)
            {
                sabotage.UseFunction();
            }
            
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(IsUsed || !_isTopView) return;
            
            cameraEvent.RaiseEvent(new FocusedSabotageEvent().Init(this, true));
            ShouldMark = false;
        }   

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isTopView) return;
            
            cameraEvent.RaiseEvent(new FocusedSabotageEvent().Init(this, false));;
        }
        
        private void OnDestroy()
        {
            cameraEvent.RemoveListener<TopViewEvent>(HandleOpen);
            cameraEvent.RaiseEvent(new RegisterSabotageEvent().Init(this, false));
        }
    }
}
