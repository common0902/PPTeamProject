using System.Numerics;
using _Script.ScriptableObject.Event;
using Quaternion = UnityEngine.Quaternion;

namespace _Works._CJW.Scripts.Events
{
    public static class SabotageEvents
    {
        public static FireSabotageEvent FireSabotageEvent = new();
    }

    public abstract class AbstractSabotageEvent : GameEvent
    {
        public bool IsUsed; // 사보타지가 사용되었는지 여부
        
        public AbstractSabotageEvent Init(bool used)
        {
            IsUsed = used;
            return this;
        }
    }

    public class FireSabotageEvent : AbstractSabotageEvent
    {
        
    }
}