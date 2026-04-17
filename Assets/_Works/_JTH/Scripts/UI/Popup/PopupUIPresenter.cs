using HwanLib.MVP.System;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIPresenter : BasePresenter
    {
        private PopupUIView _popupView;

        public override void InitializePresenter(MVPDataSO dataSO)
        {
            base.InitializePresenter(dataSO);
            
            _popupView = View as PopupUIView;
        }

        public void SetPopupTitle(string title)
        {
            
        }
        
        public void SetPopupMessage(string msg)
        {
            
        }
    }
}