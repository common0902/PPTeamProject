using HwanLib.MVP.System;

namespace HwanLib.MVP.UIData
{
    public class UIStringParam : ChangedData
    {
        public string Value;

        public UIStringParam Init(string value)
        {
            Value = value;

            return this;
        }
    }
}