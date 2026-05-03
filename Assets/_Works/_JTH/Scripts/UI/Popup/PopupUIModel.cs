using System;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;
using UnityEngine;

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
    
        private void I_YesButtonClickHandler(ChangedData _)
        {
            _yesAction?.Invoke();
        }
        
        private void I_NoButtonClickHandler(ChangedData _)
        {
            _noAction?.Invoke();
        }

        private ChangedData U_ChangePopupMessageHandler()
        {
            return UIParamData.UIStringParam.Init(_message);
        }
    }
}
