using HwanLib.MVP.System.AbstractMVP;

namespace _Works._JTH.Scripts.UI.InteractiveBtn
{
    public class InteractiveBtnUIView : AbstractPopupView
    {
        protected override int WindowFormIndex => (int)InteractiveBtnUIEnum.Window;
        protected override int BackgroundFormIndex => -1;
        protected override bool UseBackgroundForm => false;
    }
}