using System;
using _Script.SaveSystem;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIModel : ISaveableModel
    {
        private int _savedStage;
        private const string GameOverTitle = "GAME OVER";
        private const string GameClearTitle = "CLEAR";
        private bool _isGameOver;
        private int _nextStage;

        public void SetGame(bool isGameOver, int saveDataId)
        {
            _isGameOver = isGameOver;
            _nextStage = SceneManager.GetActiveScene().buildIndex - StageInfoSO.stageStartIdx + 2;
            if (_nextStage < 1 || isGameOver || _savedStage >= _nextStage || _nextStage > StageInfoSO.stageCount)
                return;
            
            SaveChannel.RaiseEvent(SaveEvents.SyncDataEvent
                    .Init(saveDataId, _nextStage.ToString()));
        }
        
        public int StageStartIndex { get; set; }
        public Action CloseViewAction { get; set; }
        public EventChannelSO OpenUIChannel { get; set; }
        public EventChannelSO SaveChannel { get; set; }
        public StageInfoSO StageInfoSO { get; set; }

        
        public void SetDefaultValue()
        {
        }

        public string StoreData()
        {
            return _savedStage.ToString();
        }

        public void RestoreData(string data)
        {
            _savedStage = int.Parse(data);
        }

        private UIParam UpdateTitleText() => UIParams.UIStringParam.Init(_isGameOver ? GameOverTitle : GameClearTitle);

        private UIParam UpdateDayText() => UIParams.UIStringParam.Init((_isGameOver ? _savedStage : _savedStage - 1).ToString());
        
        private UIParam UpdateDescText() => UIParams.UIStringParam.Init((_isGameOver ? _savedStage : _savedStage - 1).ToString());

        private void ContinueBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(_nextStage - 1, false, _savedStage != 0));
        }

        private void NextBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(_nextStage - 1, false, true));
        }

        private void QuitBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent.Init(0, false, false));
        }
    }
}