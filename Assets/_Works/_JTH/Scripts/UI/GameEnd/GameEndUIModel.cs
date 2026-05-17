using System;
using _Script.SaveSystem;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.UIData;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIModel : ISaveableModel
    {
        private int _savedStage;
        private const string GameOverTitle = "GAME OVER";
        private const string GameClearTitle = "CLEAR";
        private bool _isGameOver;

        public void SetGame(bool isGameOver, int saveDataId)
        {
            _isGameOver = isGameOver;
            if (isGameOver == false)
                SaveChannel.RaiseEvent(SaveEvents.SyncDataEvent.Init(saveDataId, (_savedStage + 1).ToString()));
        }
        
        public int StageStartIndex { get; set; }
        public Action CloseViewAction { get; set; }
        public EventChannelSO OpenUIChannel { get; set; }
        public EventChannelSO SaveChannel { get; set; }

        
        public void SetDefaultValue()
        {
            _savedStage = 0;
            _isGameOver = true;
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
                .Init(StageStartIndex + _savedStage - 1, false, _savedStage != 0));
        }

        private void NextBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(StageStartIndex + _savedStage - 1, false, true));
        }

        private void QuitBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent.Init(0, false, false));
        }
    }
}