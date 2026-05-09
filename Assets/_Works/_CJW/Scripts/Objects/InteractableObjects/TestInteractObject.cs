using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    public class TestInteractObject : AbstractInteractableObject
    {
        [SerializeField] private EventChannelSO testEvent;
        public override void HandleInteract()
        {
            base.HandleInteract();
        }
    }
}