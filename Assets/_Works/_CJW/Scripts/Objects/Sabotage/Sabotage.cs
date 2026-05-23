using System;
using System.Reflection;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.InteractableObjects;
using _Works._CJW.Scripts.Objects.Sabotage.Functions;
using UnityEngine;
using UnityEngine.EventSystems;
using static _Works._CJW.Scripts.Objects.Sabotage.SabotageVisualModule;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class Sabotage : ModuleOwner, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Sabotage Data")]
        [field: SerializeField] public SabotageDataSo SabotageData { get; private set; } 

        [Header("Event Channel")]
        [SerializeField] private EventChannelSO cameraEvent;
        [SerializeField] private EventChannelSO sabotageEvent;
        [SerializeField] private EventChannelSO interactEvent; 
        [Header("Target Event")]
        [SerializeField] public string targetEventName;
        [Header("Mark Offset")]
        [SerializeField] public Vector3 markOffset; 
        [SerializeField] public Vector2 markBoxSize; 

        [Header("Together Sabotages")]
        [SerializeField] private Sabotage[] sabotages;
        
        [field: SerializeField] public bool IsLocked { get; private set; } = false; 
                
        public bool ShouldMark { get; private set; } = true; 
        public bool IsUsed { get; private set; } = false;

        private bool _isTopView;
        private AbstractSabotageEvent _targetEvent;
        private SabotageVisualModule _visual;
        private ISabotageFunctionModule _functionModule;
        
        protected override void Awake()
        {
            base.Awake();   
            cameraEvent.AddListener<TopViewEvent>(HandleOpen);
            _visual = GetModule<SabotageVisualModule>();
            _functionModule = GetModule<ISabotageFunctionModule>();
        }

        private void Start()
        {
            _targetEvent = typeof(SabotageEvents).GetField(targetEventName,
                BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as AbstractSabotageEvent;
            cameraEvent.RaiseEvent(new RegisterSabotageEvent().Init(this, true));
            
            if (_visual != null)
            {
                _visual.HandleActivation(true);
                _visual.HandleOutLineEnable(false);
                UpdateVisualState();
            }
        }
        private void UpdateVisualState()
        {
            if (_visual == null) return;

            if (!_isTopView)
            {
                _visual.HandleOutLineEnable(false);
                return;
            }

            _visual.HandleOutLineEnable(true);

            if (IsUsed || IsLocked)
            {
                _visual.SetOutlineState(OutlineState.LOCKED);
            }
            else
            {
                _visual.SetOutlineState(OutlineState.DEFAULT);
            }
        }
        public void UnlockSabotage()
        {
            IsLocked = false;
            UpdateVisualState();
        }        
        public void LockSabotage()
        {
            IsLocked = true;
            UpdateVisualState();
        }

        private void HandleOpen(TopViewEvent evt)
        {
            _isTopView = evt.IsTopView;
            UpdateVisualState();
        }

        public void ActiveVisual(bool showVisual)
            => _visual.HandleActivation(showVisual);

        public void UseFunction()
        {
            if (IsUsed || IsLocked) return; 
    
            IsUsed = true; 

            UpdateVisualState();

            _functionModule.UseFunction();
            sabotageEvent.RaiseEvent(_targetEvent.Init(true));
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(IsUsed || IsLocked) return;

            UseFunction();
            foreach (var sabotage in sabotages)
            {
                if (sabotage != null) 
                    sabotage.UseFunction();
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"used: {IsUsed}/ locked: {IsLocked}/ topView: {_isTopView}");
            if(IsUsed || IsLocked || !_isTopView) return;

            Debug.Log("ASD");
            if (_visual != null)
            {
                _visual.SetOutlineState(OutlineState.INTERACTED);
            }
            ShouldMark = false;
        }   

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsUsed || IsLocked || !_isTopView) return;
            
            UpdateVisualState();
        }
        
        private void OnDestroy()
        {
            cameraEvent.RemoveListener<TopViewEvent>(HandleOpen);
            cameraEvent.RaiseEvent(new RegisterSabotageEvent().Init(this, false));
        }
    }
}