using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.GameEnd
{
    public class GameEndUIView : BaseView
    {
        private enum UIState
        {
            GameOver,
            GameClear,
        }

        private AccessForm _clearUI;
        private AccessForm _gameOverUI;
        private BackgroundForm _background;
        private TextForm _dayTextForm;
        private TextForm _descTextForm;
        
        public float FadeDuration { get; set; }
        public float FadeAlpha { get; set; }
        
        public void SetGameState(bool isGameOver) => UpdateState(isGameOver ? UIState.GameOver : UIState.GameClear);

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _clearUI = GetForm<AccessForm>((int)GameEndUIEnum.ClearUI);
            _gameOverUI = GetForm<AccessForm>((int)GameEndUIEnum.GameOverUI);
            _background = GetForm<BackgroundForm>((int)GameEndUIEnum.Background);
            _dayTextForm = GetForm<TextForm>((int)GameEndUIEnum.DayText);
            _descTextForm = GetForm<TextForm>((int)GameEndUIEnum.DescText);

            _background.CompleteOnStart = false;
            _background.ResetOnStart = true;
            _background.OnFadeEnd += FadeEndHandler;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            _background.OnFadeEnd -= FadeEndHandler;
        }

        private void FadeEndHandler(bool fadeIn)
        {
            if (fadeIn == false)
                CloseView();
        }

        public override void OpenView()
        {
            base.OpenView();

            if (_dayTextForm.Text.Contains("0"))
            {
                _dayTextForm.gameObject.SetActive(false);
                _descTextForm.gameObject.SetActive(false);
            }
            else
            {
                _dayTextForm.gameObject.SetActive(true);
                _descTextForm.gameObject.SetActive(true);
            }
            
            _background.DoFade(true, FadeDuration, FadeAlpha);
        }

        public void StartClose()
        {
            _background.DoFade(false, FadeDuration / 2, FadeAlpha);
        }

        private void UpdateState(UIState state)
        {
            _clearUI.gameObject.SetActive(false);
            _gameOverUI.gameObject.SetActive(false);
            
            switch (state)
            {
                case UIState.GameOver:
                    _gameOverUI.gameObject.SetActive(true);
                    break;
                case UIState.GameClear:
                    _clearUI.gameObject.SetActive(true);
                    break;
            }
        }
    }
}