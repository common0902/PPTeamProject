using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects
{
    public class TestInteractObject : AbstractInteractableObject
    {
        [SerializeField] private EventChannelSO testEvent;
        protected override void HandleTriggerEnterEvent()
        {
            base.HandleTriggerEnterEvent();
        }

        protected override void HandleInteract(GameEvent evt)
        {
            base.HandleInteract(evt);
            testEvent.RaiseEvent(InteractEvent.FireSabotage.Init(true));
        }

        protected override void HandleTriggerExitEvent()
        {
            base.HandleTriggerExitEvent();
        }
    }
}