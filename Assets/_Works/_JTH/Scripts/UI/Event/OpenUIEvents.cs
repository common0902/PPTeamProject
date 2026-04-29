using System;
using _Script.ScriptableObject.Event;

namespace _Works._JTH.Scripts.UI.Event
{
    public static class OpenUIEvents
    {
        public static readonly OpenPopupEvent OpenPopupEvent = new OpenPopupEvent();
    }

    public class OpenPopupEvent : GameEvent
    {
        public string Message;
        public Action YesAction;
        public Action NoAction;

        public OpenPopupEvent Init(string message, Action yesAction, Action noAction)
        {
            Message = message;
            YesAction = yesAction;
            NoAction = noAction;

            return this;
        }
    }
}