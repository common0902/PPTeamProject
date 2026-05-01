namespace _Script.ScriptableObject.Event
{
    public static class SaveEvents
    {
        public static readonly StoreDataEvent StoreDataEvent = new StoreDataEvent();
        public static readonly RestoreDataEvent RestoreDataEvent = new RestoreDataEvent();
    }

    public class StoreDataEvent : GameEvent
    {
        //비워둔다.
    }

    public class RestoreDataEvent : GameEvent
    {
        //이것두 비워둔다.
    }
}