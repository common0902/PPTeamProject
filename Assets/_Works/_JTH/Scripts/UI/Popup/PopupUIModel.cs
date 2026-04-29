using System;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIModel : IModel
    {
        private Action _yesAction;
        private Action _noAction;
        private string _message;
        
        public void SetActions(Action yesAction, Action noAction)
        {
            _yesAction = yesAction;
            _noAction = noAction;
        }

        public void SetMessage(string msg) => _message = msg;
        
        private ChangedData NoButtonClickHandler(ChangedData _)
        {
            _yesAction?.Invoke();
            return null;
        }
    
        private ChangedData YesButtonClickHandler(ChangedData _)
        {
            _noAction?.Invoke();
            return null;
        }

        private ChangedData ChangePopupMessageHandler(ChangedData _)
        {
            return ChangePopupMessageHandler();
        }
        
        private ChangedData ChangePopupMessageHandler()
        {
            UIParamData.UIStringParam.Value = _message;
            return UIParamData.UIStringParam;
        }
    }
}
