using System;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.UIData;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIModel : ISaveableModel
    {
        private string _savedStage;
        private const string GameOverTitle = "GAME OVER";
        private const string GameClearTitle = "CLEAR";
         
        public bool IsGameOver { get; set; }
        public int StageStartIndex { get; set; }
        public Action CloseViewAction { get; set; }
        public EventChannelSO OpenUIChannel { get; set; }
        
        private int NextStage => int.Parse(_savedStage) + 1;
        
        public void SetDefaultValue()
        {
            _savedStage = "0";
        }

        public string StoreData()
        {
            return NextStage.ToString();
        }

        public void RestoreData(string data)
            => _savedStage = data;
        
        private UIParam UpdateTitleText() => UIParams.UIStringParam.Init(IsGameOver ? GameOverTitle : GameClearTitle);

        private UIParam UpdateDayText() => UIParams.UIStringParam.Init(_savedStage);
        
        private UIParam UpdateDescText() => UIParams.UIStringParam.Init(_savedStage);

        private void ContinueBtnHandler(UIParam clickData)
            => OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(StageStartIndex + NextStage - 2, false, true));
        
        private void NextBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(StageStartIndex + NextStage - 1, true, true));
        }

        private void QuitBtnHandler(UIParam clickData)
        {
            CloseViewAction?.Invoke();
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent.Init(0, true, false));
        }
    }
}