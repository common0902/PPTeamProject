using HwanLib.MVP.System;

namespace HwanLib.MVP.UIData
{
    public class UIIntParam : ChangedData
    {
        public int Value;

        public UIIntParam Init(int value)
        {
            Value = value;
            return this;
        }
    }
}