using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private int stageStartIndex;
        [SerializeField] private float fadeDuration = 2;
        [SerializeField] private Color fadeInColor;
        [SerializeField] private EventChannelSO openUIChannel;
        
        private GameEndUIModel _gameEndModel;
        private GameEndUIView _gameEndView;
        
        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);

            _gameEndModel = (GameEndUIModel)Model;
            _gameEndView = (GameEndUIView)View;
            
            _gameEndModel.StageStartIndex = stageStartIndex;
            _gameEndModel.CloseViewAction = _gameEndView.CloseView;

            _gameEndView.FadeDuration = fadeDuration;
            _gameEndView.FadeColor = fadeInColor;
            
            openUIChannel.AddListener<OpenGameEndEvent>(OpenUI);
        }

        private void OpenUI(OpenGameEndEvent data)
        {
            _gameEndModel.IsGameOver = data.IsGameOver;
            _gameEndView.SetGameState(data.IsGameOver);
            
            _gameEndView.OpenView();
        }
    }
}