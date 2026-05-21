using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Multiable;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.UIData;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Popup
{
    public class PopupUIPresenter : BasePresenter, IMultiablePresenter
    {
        [SerializeField] private EventChannelSO openUIEvent;
        [SerializeField] private FormSoundModule<PopupUIEnum> soundModule;
        
        public event Func<IMultiablePresenter, bool> TryOpen;

        public IMultiableView MultiableView => View as IMultiableView;

        public void OpenUI()
            => _popupView.OpenView();
        
        private PopupUIView _popupView;
        private PopupUIModel _popupModel;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
                        
            _popupView = MultiableView as PopupUIView;
            _popupModel = Model as PopupUIModel;
            
            openUIEvent?.AddListener<OpenPopupEvent>(ShowPopup);
        }
        
        protected override void OnDestroy()
        {
            openUIEvent?.RemoveListener<OpenPopupEvent>(ShowPopup);
            base.OnDestroy();
        }
        
        private void ShowPopup(OpenPopupEvent eventData)
        {
            if (TryOpen == null || !TryOpen.Invoke(this))
                return;
            
            _popupModel.SetMessage(eventData.Message);
            _popupModel.SetActions(eventData.YesAction, eventData.NoAction);
            _popupView.UpdateView();
        }
        
        protected override void InteractedHandler(int childIndex, UIParam value)
        {
            base.InteractedHandler(childIndex, value);
            soundModule.PlaySound(childIndex);
        }
    }
}