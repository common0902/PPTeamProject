using _Script.ScriptableObject.Event;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.System.SaveMVP;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Title
{
    public class TitleUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private EventChannelSO saveChannel;
        [SerializeField] private int stageStartIndex = 1;
            
        private readonly string _notSavedStageIndex = "-1";
        
        private TitleUIView _titleView;
        private TitleUIModel _titleModel;

        public override void InitializePresenter(MVPDataSO dataSO)
        {
            base.InitializePresenter(dataSO);
            
            _titleView = (TitleUIView)View;
            _titleModel = (TitleUIModel)Model;

            _titleView.InitTitleView(_notSavedStageIndex);
            _titleModel.InitTitleModel(_notSavedStageIndex, stageStartIndex);
            
            _titleModel.SetPopupEventChannel(openUIChannel, saveChannel);
        }

        [ContextMenu("Save")]
        public void Save()
        {
            RestoreData("2");
            saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
        }
    }
}
