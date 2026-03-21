namespace _Script.ScriptableObject.Event
{
    public static class DataEvents
    {
        public static readonly DataSaveEvent DataSaveEvent = new DataSaveEvent();
        public static readonly DataLoadEvent DataLoadEvent = new DataLoadEvent();
    }

    public class DataSaveEvent : GameEvent
    {
        //비워둔다.
    }

    public class DataLoadEvent : GameEvent
    {
        //이것두 비워둔다.
    }
}