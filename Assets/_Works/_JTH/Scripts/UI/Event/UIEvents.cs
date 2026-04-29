using System;
using _Script.ScriptableObject.Event;

namespace _Works._JTH.Scripts.UI.Event
{
    public static class UIEvents
    {
        public static readonly OpenPopupEvent OpenPopupEvent = new OpenPopupEvent();
    }

    public class OpenPopupEvent : GameEvent
    {
        public string Message;
        public Action YesAction;
        public Action NoAction;

        public void SetValue(string message, Action yesAction, Action noAction)
        {
            Message = message;
            YesAction = yesAction;
            NoAction = noAction;
        }
    }
}