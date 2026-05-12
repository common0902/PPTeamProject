using System;
using System.Collections.Generic;
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
        [SerializeField] private int stageStartIndex = 1;
        [SerializeField] private int titleIndex;

        private TitleUIView _titleView;
        private TitleUIModel _titleModel;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
                        
            _titleView = (TitleUIView)View;
            _titleModel = (TitleUIModel)Model;

            _titleModel.InitTitleModel(stageStartIndex);
            
            _titleModel.SetPopupEventChannel(openUIChannel);

            if (SceneManager.GetActiveScene().buildIndex == titleIndex)
                _titleView.OpenView();
        }

        #if UNITY_EDITOR
        public void Save()
        {
            RestoreData("2");
            saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
        }
        #endif
    }
}
