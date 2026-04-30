namespace _Script.SaveSystem
{
    public interface IRestorable : ISaveable
    {
        void RestoreData(string data);
    }
}