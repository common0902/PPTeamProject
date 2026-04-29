using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.Title
{
    public class TitleUIView : BaseView
    {
        private enum TitleState
        {
            Default,
            Select,
            Setting,
        }

        private TitleState _currentState;
        private AccessForm _titleText;
        private AccessForm _mainButtons;
        private AccessForm _selectButtons;
        private TextButtonForm _continueButton;
        private string _notSavedStageIndex;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler);
            
            _titleText = GetForm<AccessForm>((int)TitleUIEnum.TitleText);
            _mainButtons = GetForm<AccessForm>((int)TitleUIEnum.MainButtons);
            _selectButtons = GetForm<AccessForm>((int)TitleUIEnum.SelectButtons);
            _continueButton = GetForm<TextButtonForm>((int)TitleUIEnum.ContinueBtn);

            _continueButton.InitTextButtonForm("Not Saved");
            
            AddFormInteractionListener(PlayBtnClickHandler, (int)TitleUIEnum.PlayBtn);
            AddFormInteractionListener(SettingBtnClickHandler, (int)TitleUIEnum.SettingBtn);
            
            UpdateState(TitleState.Default);
            
            OpenView();
        }
        
        public void InitTitleView(string notSavedStageIndex)
            => _notSavedStageIndex = notSavedStageIndex;

        public override void UpdateView()
        {
            _continueButton.SetInteractive(true);
            base.UpdateView();

            if (_continueButton.Text.Contains($"Day{_notSavedStageIndex}"))
            {
                _continueButton.SetInteractive(false);
            }
            else
            {
                _continueButton.SetInteractive(true);
            }
        }
        
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            RemoveFormInteractionListener(PlayBtnClickHandler, (int)TitleUIEnum.PlayBtn);
            RemoveFormInteractionListener(SettingBtnClickHandler, (int)TitleUIEnum.SettingBtn);
        }

        private void UpdateState(TitleState state)
        {
            _currentState = state;

            _titleText.gameObject.SetActive(false);
            _mainButtons.gameObject.SetActive(false);
            _selectButtons.gameObject.SetActive(false);
            switch (_currentState)
            {
                case TitleState.Default:
                    _titleText.gameObject.SetActive(true);
                    _mainButtons.gameObject.SetActive(true);
                    break;
                case TitleState.Select:
                    _selectButtons.gameObject.SetActive(true);
                    break;
                case TitleState.Setting:
                    break;
            }
        }

        private void PlayBtnClickHandler() => UpdateState(TitleState.Select);
        private void SettingBtnClickHandler() => UpdateState(TitleState.Setting);
    }
}
