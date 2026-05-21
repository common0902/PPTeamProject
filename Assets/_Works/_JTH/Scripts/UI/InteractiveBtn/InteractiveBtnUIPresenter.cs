using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Multiable;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.InteractiveBtn
{
    public class InteractiveBtnUIPresenter : BasePresenter, IMultiablePresenter
    {
        [SerializeField] private EventChannelSO openUIChannel;
        public event Func<IMultiablePresenter, bool> TryOpen;

        public IMultiableView MultiableView => View as IMultiableView;
        public override bool IsWorldPosition => true;

        private InteractiveBtnUIView _interactiveBtnUIView;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            _interactiveBtnUIView = View as InteractiveBtnUIView;
            _interactiveBtnUIView.OpenDuration = 0.1f;

            openUIChannel.AddListener<ObjectRegisterEvent>(SabotageInteractiveHandler);
        }

        private void SabotageInteractiveHandler(ObjectRegisterEvent data)
        {
            if (data.IsRegistered == false)
            {
                _interactiveBtnUIView.CloseView();
                return;
            }
            TryOpen?.Invoke(this);
        }

        public void OpenUI()
            => _interactiveBtnUIView.OpenView();
    }
}