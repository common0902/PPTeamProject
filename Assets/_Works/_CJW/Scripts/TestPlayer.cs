using System;
using _Script.Agent;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts
{
    public class TestPlayer : Agent
    {
        [SerializeField] private EventChannelSO interactEvent;
        
        private void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                interactEvent.RaiseEvent(InteractEvents.InteractEvent.Init(true));
            }
        }

        protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
        {
            
        }
    }
}