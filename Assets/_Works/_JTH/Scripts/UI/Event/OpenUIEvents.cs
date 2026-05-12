using System;
using _Script.ScriptableObject.Event;

namespace _Works._JTH.Scripts.UI.Event
{
    public static class OpenUIEvents
    {
        public static readonly OpenPopupEvent OpenPopupEvent = new OpenPopupEvent();
        public static readonly OpenSettingEvent OpenSettingEvent = new OpenSettingEvent();
        public static readonly OpenTooltipEvent OpenTooltipEvent = new OpenTooltipEvent();
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

    public class OpenSettingEvent : GameEvent
    {
        
    }
    
    public class OpenTooltipEvent : GameEvent
    {
        public string TitleText;
        public string DescText;

        public OpenTooltipEvent Init(string titleText, string descText)
        {
            TitleText = titleText;
            DescText = descText;
            
            return this;
        }
    }
}