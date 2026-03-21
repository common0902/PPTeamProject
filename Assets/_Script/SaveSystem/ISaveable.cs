namespace _Script.SaveSystem
{
    public interface ISaveable
    {
        SaveData SaveId { get; }
        string GetSaveData();
        void RestoreData(string data);
    }
}