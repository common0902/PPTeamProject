using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;

namespace _Works._JTH.Scripts.UI.FadeUI
{
    public class FadeUIModel : IModel
    {
        public int CurrentStage { get; set; }
        
        private UIParam UpdateStageText()
            => UIParams.UIStringParam.Init(CurrentStage.ToString());
    }
}