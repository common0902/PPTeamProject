namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class AbstractSaveDataModel : IModel
    {
        public abstract void LoadData();

        public abstract void SaveData();
    }
}