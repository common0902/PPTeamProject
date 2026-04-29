using System;
using _Script.ScriptableObject.Event;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] private EventChannelSO interactEvent;

        private void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                interactEvent.RaiseEvent(new GameEvent());
            }
        }
    }
}