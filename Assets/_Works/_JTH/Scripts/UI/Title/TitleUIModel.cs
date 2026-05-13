using System;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System;
using HwanLib.MVP.System.SaveMVP;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.Title
{
    public class TitleUIModel : ISaveableModel
    {
        private string _savedStage;

        private EventChannelSO _openUIChannel;
        private EventChannelSO _saveChannel;
        private int _stageStartIndex;

        public void InitTitleModel(int stageStartIndex)
            => _stageStartIndex = stageStartIndex;

        public void SetDefaultValue(EventChannelSO saveChannel)
        {
            _saveChannel = saveChannel;
            _savedStage = "-1";
        }

        public string StoreData()
        {
            return _savedStage;
        }

        public void RestoreData(string data)
        {
            _savedStage = data;
        }

        public void SetPopupEventChannel(EventChannelSO openUIChannel)
        {
            _openUIChannel = openUIChannel;
        }
        
        private void NewGameBtnClickHandler(UIParam clickData)
        {
            _openUIChannel.RaiseEvent(
                OpenUIEvents.OpenPopupEvent.Init("모든 데이터가 사라집니다. 새 게임을 시작하시겠습니까?"
                    , OpenTutorialPopup, () => { }));
        }

        private void OpenTutorialPopup()
        {
            _openUIChannel.RaiseEvent(
                OpenUIEvents.OpenPopupEvent.Init("튜토리얼을 진행하시겠습니까?"
                    , () =>
                    {
                        _savedStage = "-1";
                        _saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
                        SceneManager.LoadScene(_stageStartIndex - 1);
                    }, () =>
                    {
                        SceneManager.LoadScene(_stageStartIndex);
                    }));
        }
        
        private UIParam ContinueTextHandler()
        {
            return UIParamContainer.UIStringParam.Init(
                !String.IsNullOrEmpty(_savedStage) ? _savedStage : "-1");
        }
        
        private void ContinueBtnHandler(UIParam clickData)
        {
            SceneManager.LoadScene(int.Parse(_savedStage));
        }
        
        private void QuitBtnClickHandler(UIParam clickData)
        {
            _openUIChannel.RaiseEvent(OpenUIEvents.OpenPopupEvent.Init("게임을 종료하시겠습니까?"
                , () =>
                {
                    _saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
                    Application.Quit();
                }, () => { }));
        }
        
        private void SettingBtnClickHandler(UIParam clickData)
        {
            _openUIChannel.RaiseEvent(OpenUIEvents.OpenSettingEvent);
        }
    }
}