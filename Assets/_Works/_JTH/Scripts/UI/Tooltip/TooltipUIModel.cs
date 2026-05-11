using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;

namespace _Works._JTH.Scripts.UI.Tooltip
{
    public class TooltipUIModel : IModel
    {
        private string _titleText;
        private string _descText;
        
        public void SetText(string titleText, string descText)
        {
            _titleText = titleText;
            _descText = descText;
        }
        
        private UIParam UpdateTitleText()
            => UIParamContainer.UIStringParam.Init(_titleText);
        
        private UIParam UpdateDescText()
            => UIParamContainer.UIStringParam.Init(_descText);
    }
}