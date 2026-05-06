using System;
using HwanLib.MVP.System.BaseMVP;

namespace HwanLib.MVP.System.SaveMVP
{
    public abstract class AbstractSaveableModel : IModel
    {
        public abstract void SetDefaultData();
        public abstract string StoreData();
        public abstract void RestoreData(string data);
    }
}