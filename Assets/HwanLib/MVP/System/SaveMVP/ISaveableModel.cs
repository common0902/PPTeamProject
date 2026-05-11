using _Script.ScriptableObject.Event;
using HwanLib.MVP.System.BaseMVP;

namespace HwanLib.MVP.System.SaveMVP
{
    public interface ISaveableModel : IModel
    {
        public void SetDefaultValue(EventChannelSO saveChannel);
        public string StoreData();
        public void RestoreData(string data);
    }
}