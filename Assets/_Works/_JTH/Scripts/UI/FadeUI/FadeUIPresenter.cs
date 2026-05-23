using System;
using System.Collections;
using System.Collections.Generic;
using _Script.SaveSystem;
using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using _Works._JTH.Scripts.UI.Event;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts.UI.FadeUI
{
    public class FadeUIPresenter : BasePresenter
    {
        [SerializeField] private EventChannelSO saveChannel;
        [SerializeField] private EventChannelSO openUIChannel;
        [SerializeField] private StageInfoSO stageInfoSO;
        [SerializeField] private float fadeDuration = 2;
        [SerializeField] private float drawDuration = 2;
        [SerializeField] private float waitForCloseDuration = 0.25f;
        
        private FadeUIModel _fadeModel;
        private FadeUIView _fadeView;
        
        private int _nextStageIndex;
        
        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);

            _fadeModel = (FadeUIModel)Model;
            _fadeView = (FadeUIView)View;
            
            _fadeView.FadeDuration = fadeDuration;
            _fadeView.DrawDuration = drawDuration;
            
            openUIChannel.AddListener<OpenFadeUIEvent>(OpenUI);
            _fadeView.BackgroundForm.OnFadeEnd += CompleteFadeHandler;
        }

        private void SceneLoadedHandler(Scene _, LoadSceneMode __)
            => StartCoroutine(WaitForCloseView());

        private IEnumerator WaitForCloseView()
        {
            yield return new WaitForSecondsRealtime(waitForCloseDuration);
            _fadeView.StartClose();
        }

        private void OpenUI(OpenFadeUIEvent data)
        {
            _nextStageIndex = data.SceneIndex;
            _fadeView.NextStage = _nextStageIndex.ToString();
            _fadeView.DrawNextDayText = data.DrawNextDayText;
            _fadeView.DrawCurDayText = data.DrawCurDayText;

            _fadeModel.CurrentStage = SceneManager.GetActiveScene().buildIndex - stageInfoSO.stageStartIdx + 1;
            StopAllCoroutines();
            _fadeView.OpenView();
            
            saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
        }

        private void CompleteFadeHandler(bool fadeIn)
        {
            if (fadeIn)
            {
                SceneManager.LoadScene(_nextStageIndex);
                SceneManager.sceneLoaded += SceneLoadedHandler;
            }
            else
            {
                _fadeView.CloseView();
                SceneManager.sceneLoaded -= SceneLoadedHandler;
            }
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _fadeView.BackgroundForm.OnFadeEnd -= CompleteFadeHandler;
        }
    }
}