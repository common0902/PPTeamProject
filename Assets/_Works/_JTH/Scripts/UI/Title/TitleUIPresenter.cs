using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
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

            _titleModel.StageStartIndex = stageStartIndex;
            _titleModel.PopupCloseEvent = _titleView.CloseView;
            _titleModel.OpenUIChannel = openUIChannel;

            if (SceneManager.GetActiveScene().buildIndex == titleIndex)
                _titleView.OpenView();
        }
    }
}
