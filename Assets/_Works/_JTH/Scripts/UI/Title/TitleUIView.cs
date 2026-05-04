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
        }

        private TitleState _currentState;
        private AccessForm _titleText;
        private AccessForm _mainButtons;
        private AccessForm _selectButtons;
        private TextButtonForm _continueTxtBtn;

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _titleText = GetForm<AccessForm>((int)TitleUIEnum.TitleText);
            _mainButtons = GetForm<AccessForm>((int)TitleUIEnum.MainButtons);
            _selectButtons = GetForm<AccessForm>((int)TitleUIEnum.SelectButtons);
            _continueTxtBtn = GetForm<TextButtonForm>((int)TitleUIEnum.ContinueTxtBtn);
            
            _continueTxtBtn.SetTextAndButtonForm(GetForm<TextForm>((int)TitleUIEnum.ContinueTxt),
                GetForm<FadeButtonForm>((int)TitleUIEnum.ContinueBtn));
            _continueTxtBtn.SetInteractiveFalseText("Not Saved");
            
            AddFormInteractionListener(PlayBtnClickHandler, (int)TitleUIEnum.PlayBtn);
            AddFormInteractionListener(BackgroundClickHandler, (int)TitleUIEnum.SelectBackground);
            
            UpdateState(TitleState.Default);
            
            OpenView();
        }
        
        public override void UpdateView()
        {
            base.UpdateView();

            if (_continueTxtBtn.Text.Contains("Day-1"))
            {
                _continueTxtBtn.SetInteractive(false);
            }
            else
            {
                _continueTxtBtn.SetInteractive(true);
            }
        }
        
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            RemoveFormInteractionListener(PlayBtnClickHandler, (int)TitleUIEnum.PlayBtn);
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
            }
        }

        private void PlayBtnClickHandler() => UpdateState(TitleState.Select);
        private void BackgroundClickHandler() => UpdateState(TitleState.Default);
    }
}
