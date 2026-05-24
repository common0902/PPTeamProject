using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Multiable;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.InteractiveBtn
{
    public class InteractiveBtnUIPresenter : BasePresenter, IMultiablePresenter
    {
        [SerializeField] private EventChannelSO interactObjectChannel;
        public event Func<IMultiablePresenter, bool> TryOpen;

        public IMultiableView MultiableView => View as IMultiableView;
        public override bool IsWorldPosition => true;

        private InteractiveBtnUIView _interactiveBtnUIView;
        private Transform _targetObjectTrm;
        private Camera _mainCam;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            _interactiveBtnUIView = View as InteractiveBtnUIView;
            
            _interactiveBtnUIView.OpenDuration = 0.1f;
            _interactiveBtnUIView.CloseDuration = 0.1f;

            interactObjectChannel.AddListener<ObjectRegisterEvent>(SabotageInteractiveHandler);
            SceneManager.sceneLoaded += GetMainCamera;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= GetMainCamera;
            interactObjectChannel.RemoveListener<ObjectRegisterEvent>(SabotageInteractiveHandler);
        }

        private void GetMainCamera(Scene _, LoadSceneMode __)
            => _mainCam = Camera.main;

        private void SabotageInteractiveHandler(ObjectRegisterEvent data)
        {
            if (data.IsRegistered == false)
            {
                if (data.InteractableObject.UiShowPos == _targetObjectTrm)
                {
                    _interactiveBtnUIView.CloseView();
                    _targetObjectTrm = null;
                }

                return;
            }

            if (TryOpen?.Invoke(this) is true)
            {
                _targetObjectTrm = data.InteractableObject.UiShowPos;
            }
        }

        private void Update()
        {
            if (_targetObjectTrm != null)
                _interactiveBtnUIView.MoveToTargetTransform(_mainCam, _targetObjectTrm);
        }

        public void OpenUI()
            => _interactiveBtnUIView.OpenView();
    }
}