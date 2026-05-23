using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.UIData;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private StageInfoSO stageInfoSO;
        [SerializeField] private float fadeDuration = 2;
        [SerializeField] private float fadeInAlpha;
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private FormSoundModule<GameEndUIEnum> soundModule;
        
        private GameEndUIModel _gameEndModel;
        private GameEndUIView _gameEndView;
        
        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);

            _gameEndModel = (GameEndUIModel)Model;
            _gameEndView = (GameEndUIView)View;
            
            _gameEndModel.StageStartIndex = stageInfoSO.stageStartIdx;
            _gameEndModel.CloseViewAction = _gameEndView.StartClose;
            _gameEndModel.OpenUIChannel = openUIChannel;
            _gameEndModel.SaveChannel = saveChannel;
            _gameEndModel.StageInfoSO = stageInfoSO;

            _gameEndView.FadeDuration = fadeDuration;
            _gameEndView.FadeAlpha = fadeInAlpha;
            
            openUIChannel.AddListener<OpenGameEndEvent>(OpenUI);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            openUIChannel.RemoveListener<OpenGameEndEvent>(OpenUI);
        }

        private void OpenUI(OpenGameEndEvent data)
        {
            _gameEndModel.SetGame(data.IsGameOver, SaveId.Id);
            _gameEndView.SetGameState(data.IsGameOver);
            
            Time.timeScale = 0;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            _gameEndView.OpenView();
        }
        
        protected override void InteractedHandler(int childIndex, UIParam value)
        {
            base.InteractedHandler(childIndex, value);
            soundModule.PlaySound(childIndex);
        }
    }
}