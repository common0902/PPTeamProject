using System;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class SabotageVisualModule : MonoBehaviour, IModule
    {
        public enum OutlineState
        {
            DEFAULT,      // 기본 상태
            LOCKED,       // 잠김 상태
            INTERACTED    // 상호작용 
        }
        [SerializeField] private EventChannelSO cameraEvent;
        
        [SerializeField] private Color defaultOutLineColor;      
        [SerializeField] private Color lockedOutLineColor;       
        [SerializeField] private Color interactedOutLineColor;   
        
        [SerializeField] private GameObject visualObject;
        
        private Outline _outline;
        private ModuleOwner _owner;
        private bool _isTopView = false;
        

        public void Initialize(ModuleOwner moduleOwner)
        {
            _outline = visualObject.GetComponent<Outline>();
            _owner = moduleOwner;
        }
        

        public void HandleActivation(bool showVisual)
        {
            if (visualObject != null)
                visualObject.SetActive(showVisual);
        }

        public void HandleOutLineEnable(bool enable)
        {
            if (_outline == null) return;
            _outline.enabled = enable;
        }
        public void SetOutlineState(OutlineState state)
        {
            if (_outline == null) return;

            switch (state)
            {
                case OutlineState.LOCKED:
                    _outline.OutlineColor = lockedOutLineColor;
                    break;
                case OutlineState.INTERACTED:
                    _outline.OutlineColor = interactedOutLineColor;
                    break;
                case OutlineState.DEFAULT:
                default:
                    _outline.OutlineColor = defaultOutLineColor;
                    break;
            }
        }
    }
}