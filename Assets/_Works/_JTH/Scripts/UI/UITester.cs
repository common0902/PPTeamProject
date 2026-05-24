using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI
{
    public class UITester : LightSingleton<UITester>
    {
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private EventChannelSO interactChannel;

        private void Update()
        {
            if (Keyboard.current.digit4Key.wasPressedThisFrame)
                SceneManager.LoadScene(2);
            if (Keyboard.current.digit5Key.wasPressedThisFrame)
                SceneManager.LoadScene(3);
            if (Keyboard.current.digit6Key.wasPressedThisFrame)
                SceneManager.LoadScene(4);
            if (Keyboard.current.digit7Key.wasPressedThisFrame)
                SceneManager.LoadScene(5);
            if (Keyboard.current.digit8Key.wasPressedThisFrame)
                SceneManager.LoadScene(6);
            if (Keyboard.current.digit9Key.wasPressedThisFrame)
                SceneManager.LoadScene(7);
            if (Keyboard.current.digit0Key.wasPressedThisFrame)
                SceneManager.LoadScene(8);
        }
    }
}