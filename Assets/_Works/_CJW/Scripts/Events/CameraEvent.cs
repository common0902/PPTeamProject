using _Script.ScriptableObject.Event;

namespace _Works._CJW.Scripts.Events
{
    public static class CameraEvent
    {
        public static TopViewEvent TopViewEvent = new();
    }

    public class TopViewEvent : GameEvent
    {
        public bool IsTopView = false;

        public TopViewEvent Init(bool isTopView)
        {
            IsTopView = isTopView;
            return this;
        }
    }
}