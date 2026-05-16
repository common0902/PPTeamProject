using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JTH.Scripts.UI
{
    public class UITester : MonoBehaviour
    {
        [SerializeField] private EventChannelSO openUIChannel;

        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent.Init(3, false, true));
            else if (Keyboard.current.wKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent(OpenUIEvents.OpenGameEndEvent.Init(false));
            else if (Keyboard.current.eKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent(OpenUIEvents.OpenPopupEvent.Init("안녕하세용", () => Debug.Log("안녕하세요?"), () => Debug.Log("안 안녕하세요?")));
            else if (Keyboard.current.rKey.wasPressedThisFrame)
                openUIChannel.RaiseEvent((OpenUIEvents.OpenSettingEvent));
        }
    }
}