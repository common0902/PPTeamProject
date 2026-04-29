using _Script.ScriptableObject.Event;

namespace _Works._CJW.Scripts.Events
{
    public static class InteractEvent
    {
        public static readonly FireSabotageEvent FireSabotage = new();
    }

    public class FireSabotageEvent : GameEvent
    {
        public bool IsInteracted { get; private set; }
        
        public FireSabotageEvent Init(bool isInteracted)
        {
            IsInteracted = isInteracted;
            return this;
        }
    }
}