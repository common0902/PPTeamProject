using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private StageInfoSO stageInfoSO;
        [SerializeField] private float fadeDuration = 2;
        [SerializeField] private float fadeInAlpha;
        [SerializeField] private EventChannelSO openUIChannel;
        
        private GameEndUIModel _gameEndModel;
        private GameEndUIView _gameEndView;
        
        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);

            _gameEndModel = (GameEndUIModel)Model;
            _gameEndView = (GameEndUIView)View;
            
            _gameEndModel.StageStartIndex = stageInfoSO.stageStart;
            _gameEndModel.CloseViewAction = _gameEndView.StartClose;
            _gameEndModel.OpenUIChannel = openUIChannel;
            _gameEndModel.SaveChannel = saveChannel;

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
            
            _gameEndView.OpenView();
        }
    }
}