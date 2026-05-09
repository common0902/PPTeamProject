using _Script.ScriptableObject.Event;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.System.SaveMVP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.Title
{
    public class TitleUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private EventChannelSO saveChannel;
        [SerializeField] private int stageStartIndex = 1;
        [SerializeField] private int titleIndex = 0;

        private TitleUIView _titleView;
        private TitleUIModel _titleModel;

        public override void InitializePresenter(MVPDataSO dataSO)
        {
            base.InitializePresenter(dataSO);
            
            _titleView = (TitleUIView)View;
            _titleModel = (TitleUIModel)Model;

            _titleModel.InitTitleModel(stageStartIndex);
            
            _titleModel.SetPopupEventChannel(openUIChannel, saveChannel);

            if (SceneManager.GetActiveScene().buildIndex == titleIndex)
                _titleView.OpenView();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            RestoreData("2");
            saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
        }
    }
}
