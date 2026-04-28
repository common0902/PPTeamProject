using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._JYG._Script.EventChannel.UIEvent
{
    public static class IconEvents
    {
        public static readonly IconActiveOn IconActiveOn = new IconActiveOn();
        public static readonly IconActiveOff IconActiveOff = new IconActiveOff();
    }

    public class IconActiveOn : GameEvent
    {
        
    }

    public class IconActiveOff : GameEvent
    {
        
    }
}
