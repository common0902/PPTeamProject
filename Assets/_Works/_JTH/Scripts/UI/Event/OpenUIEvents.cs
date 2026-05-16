using System;
using _Script.ScriptableObject.Event;

namespace _Works._JTH.Scripts.UI.Event
{
    public static class OpenUIEvents
    {
        public static readonly OpenPopupEvent OpenPopupEvent = new OpenPopupEvent();
        public static readonly OpenSettingEvent OpenSettingEvent = new OpenSettingEvent();
        public static readonly OpenGameEndEvent OpenGameEndEvent = new OpenGameEndEvent();
        public static readonly OpenFadeUIEvent OpenFadeUIEvent = new OpenFadeUIEvent();
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
    
    public class OpenGameEndEvent : GameEvent
    {
        public bool IsGameOver;

        public OpenGameEndEvent Init(bool isGameOver)
        {
            IsGameOver = isGameOver;
            return this;
        }
    }

    public class OpenFadeUIEvent : GameEvent
    {
        public int SceneIndex;
        public bool DrawNextDayText;
        public bool DrawCurDayText;

        public OpenFadeUIEvent Init(int sceneIndex, bool  drawNextDayText, bool drawCurDayText)
        {
            SceneIndex = sceneIndex;
            DrawNextDayText = drawNextDayText;
            DrawCurDayText = drawCurDayText;
            
            return this;
        }
    }
}