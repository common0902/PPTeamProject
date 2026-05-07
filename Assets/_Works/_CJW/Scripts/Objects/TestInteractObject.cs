using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects
{
    public class TestInteractObject : AbstractInteractableObject
    {
        [SerializeField] private EventChannelSO testEvent;
        public override void HandleInteract()
        {
            base.HandleInteract();
            Debug.Log("Interact Test Debug");
        }
    }
}