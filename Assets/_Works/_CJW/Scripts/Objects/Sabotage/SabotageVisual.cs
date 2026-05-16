using System;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class SabotageVisual : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO cameraEvent;
        
        [SerializeField] private Color defaultOutLineColor;
        [SerializeField] private Color interactedOutLineColor;
        [SerializeField] private GameObject visualObject;
        [SerializeField] private GameObject lockedObject;
        
        private Outline _outline;
        private ModuleOwner _owner;
        

        public void Initialize(ModuleOwner moduleOwner)
        {
            _outline = visualObject.GetComponent<Outline>();
            _owner = moduleOwner;
            _outline.OutlineColor = defaultOutLineColor;
            HandleActivation(false, false);
            cameraEvent.AddListener<FocusedSabotageEvent>(HandleFocused);
        }

        private void HandleFocused(FocusedSabotageEvent obj)
        {
            if(obj.Sabotage.gameObject != _owner.gameObject)
                return;
            
            if (obj.IsFocused)
                HandleOutLineColor(true);
            else
                HandleOutLineColor(false);
        }

        public void HandleActivation(bool visual, bool lockVisual)
        {
            visualObject.SetActive(visual);
            lockedObject.SetActive(lockVisual);
        }

        public void HandleOutLineEnable(bool enable)
        {
            _outline.enabled = enable;
        }

        private void HandleOutLineColor(bool isSelected)
        {
            _outline.OutlineColor = isSelected ? interactedOutLineColor : defaultOutLineColor;
        }

        private void OnDestroy()
        {
            cameraEvent.RemoveListener<FocusedSabotageEvent>(HandleFocused);
        }
    }
}