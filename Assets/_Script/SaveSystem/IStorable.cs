namespace _Script.SaveSystem
{
    public interface IStorable : ISaveable
    {
        string GetSaveData();
    }
}