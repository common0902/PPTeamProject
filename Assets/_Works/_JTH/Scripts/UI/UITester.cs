using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JTH.Scripts.UI
{
    public class UITester : LightSingleton<UITester>
    {
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private EventChannelSO interactChannel;

        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent.Init(3, false, true));
            else if (Keyboard.current.wKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent(OpenUIEvents.OpenGameEndEvent.Init(false));
            else if (Keyboard.current.eKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent(OpenUIEvents.OpenGameEndEvent.Init(true));
            else if (Keyboard.current.rKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent((OpenUIEvents.OpenSettingEvent));
            else if (Keyboard.current.tKey.wasPressedThisFrame)
                interactChannel.RaiseEvent(InteractEvents.ObjectRegisterEvent
                    .Init(true, null));
            else if (Keyboard.current.yKey.wasPressedThisFrame)
                interactChannel.RaiseEvent(InteractEvents.ObjectRegisterEvent
                    .Init(false, null));
        }
    }
}