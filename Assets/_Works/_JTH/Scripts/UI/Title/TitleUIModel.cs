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
        private string _notSavedStageIndex;
        private int _stageStartIndex;

        public void InitTitleModel(string notSavedStageIndex, int stageStartIndex)
        {
            _notSavedStageIndex = notSavedStageIndex;
            _stageStartIndex = stageStartIndex;
        }

        public override string StoreData()
        {
            return SavedStage;
        }

        public override void RestoreData(string data)
        {
            SavedStage = data;
        }

        private void I_NewGameBtnClickHandler(ChangedData clickData)
        {
            _openUIChannel.RaiseEvent(
                OpenUIEvents.OpenPopupEvent.Init("모든 데이터가 사라집니다. 새 게임을 시작하시겠습니까?"
                    , () =>
                    {
                        SavedStage = _notSavedStageIndex;
                        _saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
                        SceneManager.LoadScene(_stageStartIndex);
                    }, () => { }));
        }

        private ChangedData U_ContinueBtnHandler()
        {
            return UIParamData.UIStringParam.Init(
                !String.IsNullOrEmpty(SavedStage) ? SavedStage : _notSavedStageIndex);
        }
        
        private void I_ContinueBtnHandler(ChangedData clickData)
        {
            SceneManager.LoadScene(int.Parse(SavedStage));
        }
        
        private void I_QuitBtnClickHandler(ChangedData clickData)
        {
            _openUIChannel.RaiseEvent(OpenUIEvents.OpenPopupEvent.Init("게임을 종료하시겠습니까?"
                , () => Application.Quit(), () => { }));
        }

        public void SetPopupEventChannel(EventChannelSO openUIChannel, EventChannelSO saveChannel)
        {
            _openUIChannel = openUIChannel;
            _saveChannel = saveChannel;
        }
    }
}