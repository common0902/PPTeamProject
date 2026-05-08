using HwanLib.MVP.System;

namespace HwanLib.MVP.UIData
{
    public class UISwapParam : UIParam
    {
        public int Item1;
        public int Item2;

        public void Init(int item1, int item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}