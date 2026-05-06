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
    public class TitleUIModel : AbstractSaveableModel
    {
        public string SavedStage { get; private set; }

        private EventChannelSO _openUIChannel;
        private EventChannelSO _saveChannel;
        private int _stageStartIndex;

        public void InitTitleModel(int stageStartIndex)
            => _stageStartIndex = stageStartIndex;

        public override void SetDefaultData()
        {
            SavedStage = "-1";
        }

        public override string StoreData()
        {
            return SavedStage;
        }

        public override void RestoreData(string data)
        {
            SavedStage = data;
        }

        public void SetPopupEventChannel(EventChannelSO openUIChannel, EventChannelSO saveChannel)
        {
            _openUIChannel = openUIChannel;
            _saveChannel = saveChannel;
        }
        
        private void NewGameBtnClickHandler(UIParam clickData)
        {
            _openUIChannel.RaiseEvent(
                OpenUIEvents.OpenPopupEvent.Init("모든 데이터가 사라집니다. 새 게임을 시작하시겠습니까?"
                    , () =>
                    {
                        SavedStage = "-1";
                        _saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
                        SceneManager.LoadScene(_stageStartIndex);
                    }, () => { }));
        }

        private UIParam ContinueBtnHandler()
        {
            return UIParamData.UIStringParam.Init(
                !String.IsNullOrEmpty(SavedStage) ? SavedStage : "-1");
        }
        
        private void ContinueBtnHandler(UIParam clickData)
        {
            SceneManager.LoadScene(int.Parse(SavedStage));
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
        
        private void SettingBtnClickHandler(UIParam _)
        {
            _openUIChannel.RaiseEvent(OpenUIEvents.OpenSettingEvent);
        }
    }
}