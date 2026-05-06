using System.Numerics;
using _Script.ScriptableObject.Event;
using Quaternion = UnityEngine.Quaternion;

namespace _Works._CJW.Scripts.Events
{
    public static class SabotageEvents
    {
        public static TestEvent TestEvent = new();
    }

    public class TestEvent : GameEvent
    {
        
    }
}