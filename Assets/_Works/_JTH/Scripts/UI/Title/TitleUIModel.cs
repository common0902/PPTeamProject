using System;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.Title
{
    public class TitleUIModel : ISaveableModel
    {
        private string _savedStage;
        private const string NotSavedStage = "0";

        public EventChannelSO OpenUIChannel { get; set; }
        public int StageStartIndex { get; set; }
        public Action PopupCloseEvent {get; set;}

        public void SetDefaultValue()
        {
            _savedStage = NotSavedStage;
        }

        public string StoreData()
        {
            return _savedStage;
        }

        public void RestoreData(string data)
        {
            _savedStage = data;
        }

        private void NewGameBtnClickHandler(UIParam clickData)
        {
            OpenUIChannel.RaiseEvent(
                OpenUIEvents.OpenPopupEvent.Init("모든 데이터가 사라집니다. 새 게임을 시작하시겠습니까?"
                    , OpenTutorialPopup, () => { }));
        }

        private void OpenTutorialPopup()
        {
            OpenUIChannel.RaiseEvent(
                OpenUIEvents.OpenPopupEvent.Init("튜토리얼을 진행하시겠습니까?"
                    , () =>
                    {
                        PopupCloseEvent?.Invoke();
                        _savedStage = NotSavedStage;
                        OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                            .Init(StageStartIndex - 1, false, false));
                    }, () =>
                    {
                        PopupCloseEvent?.Invoke();
                        OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                            .Init(StageStartIndex, false, true));
                    }));
        }
        
        private UIParam ContinueTextHandler()
        {
            return UIParams.UIStringParam.Init(
                !String.IsNullOrEmpty(_savedStage) ? _savedStage : NotSavedStage);
        }
        
        private void ContinueBtnHandler(UIParam clickData)
        {
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenFadeUIEvent
                .Init(int.Parse(_savedStage) - 1 + StageStartIndex, false, true));
        }
        
        private void QuitBtnClickHandler(UIParam clickData)
        {
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenPopupEvent.Init("게임을 종료하시겠습니까?"
                , Application.Quit, () => { }));
        }
        
        private void SettingBtnClickHandler(UIParam clickData)
        {
            OpenUIChannel.RaiseEvent(OpenUIEvents.OpenSettingEvent);
        }
    }
}