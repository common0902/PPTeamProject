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
        
        public float FadeDuration { get; set; }
        public Color FadeColor { get; set; }
        
        public void SetGameState(bool isGameOver) => UpdateState(isGameOver ? UIState.GameOver : UIState.GameClear);

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _clearUI = GetForm<AccessForm>((int)GameEndUIEnum.ClearUI);
            _gameOverUI = GetForm<AccessForm>((int)GameEndUIEnum.GameOverUI);
            _background = GetForm<BackgroundForm>((int)GameEndUIEnum.Background);
        }

        public override void OpenView()
        {
            base.OpenView();
            
            _background.DoFade(FadeDuration, FadeColor);
        }

        public override void CloseView()
        {
            base.CloseView();
            
            _background.DoFade(false, 0);
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