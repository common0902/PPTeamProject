using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIPresenter : BasePresenter
    {
        [SerializeField] private EventChannelSO openUIEvent;
        
        private PopupUIView _popupView;
        private PopupUIModel _popupModel;

        public override void InitializePresenter(MVPDataSO dataSO)
        {
            base.InitializePresenter(dataSO);
            
            _popupView = View as PopupUIView;
            _popupModel = Model as PopupUIModel;
            
            openUIEvent?.AddListener<OpenPopupEvent>(ShowPopup);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            openUIEvent?.RemoveListener<OpenPopupEvent>(ShowPopup);
        }

        #if UNITY_EDITOR
        [ContextMenu("ShowPopup")]
        public void TestPopup()
        {
            ShowPopup(OpenUIEvents.OpenPopupEvent
                .Init("안녕하세요?", () => Debug.Log("No"), () => Debug.Log("Yes")));
        }
        #endif
        
        private void ShowPopup(OpenPopupEvent eventData)
        {
            _popupModel.SetMessage(eventData.Message);
            _popupModel.SetActions(eventData.YesAction, eventData.NoAction);
            
            _popupView.OpenView();
        }
    }
}