using HwanLib.MVP.System.BaseMVP;

namespace HwanLib.MVP.System.AbstractMVP.SaveMVP
{
    public interface ISaveableModel : IModel
    {
        public void SetDefaultValue();
        public string StoreData();
        public void RestoreData(string data);
    }
}