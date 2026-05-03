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
    
        private void YesButtonClickHandler(UIParam _)
        {
            _yesAction?.Invoke();
        }
        
        private void NoButtonClickHandler(UIParam _)
        {
            _noAction?.Invoke();
        }

        private UIParam ChangePopupMessageHandler()
        {
            return UIParamData.UIStringParam.Init(_message);
        }
    }
}
