using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using HwanLib.MVP.System.AbstractMVP.SaveMVP;
using HwanLib.MVP.System.GenerateUI;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.Title
{
    public class TitleUIPresenter : AbstractSaveablePresenter
    {
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private StageInfoSO stageInfoSO;
        [SerializeField] private FormSoundModule<TitleUIEnum> soundModule;

        private TitleUIView _titleView;
        private TitleUIModel _titleModel;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
                        
            _titleView = (TitleUIView)View;
            _titleModel = (TitleUIModel)Model;

            _titleModel.StageStartIndex = stageInfoSO.stageStartIdx;
            _titleModel.OpenUIChannel = openUIChannel;
            _titleModel.SaveChannel = saveChannel;
            _titleModel.SaveId = SaveId.Id;

            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }

        private void SceneLoadedHandler(Scene scene, LoadSceneMode __)
        {
            if (scene.buildIndex == stageInfoSO.titleIdx)
                _titleView.OpenView();
            else
                _titleView.CloseView();
        }

        protected override void InteractedHandler(int childIndex, UIParam value)
        {
            base.InteractedHandler(childIndex, value);
            soundModule.PlaySound(childIndex);
        }
    }
}
