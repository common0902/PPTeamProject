using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.Forms.Module.DrawerModule;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.FadeUI
{
    public class FadeUIView : BaseView
    {
        public float DrawDuration { get; set; }
        public float FadeDuration { get; set; }
        public string NextStage { get; set; }
        public bool DrawCurDayText { get; set; }
        public bool DrawNextDayText { get; set; }
        
        private TextForm _stageTextForm;
        public BackgroundForm BackgroundForm { get; private set; }

        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);
            
            _stageTextForm = GetForm<TextForm>((int)FadeUIEnum.StageText);
            BackgroundForm = GetForm<BackgroundForm>((int)FadeUIEnum.Background);
            
            _stageTextForm.InitializeDrawer(DrawDirection.Up);
            _stageTextForm.DrawerModule.Draw(false, 0, true);
            _stageTextForm.DrawerModule.OnDrawEnd += DrawEndHandler;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            
            _stageTextForm.DrawerModule.OnDrawEnd -= DrawEndHandler;
        }

        private void DrawEndHandler(bool isIn)
        {
            //바깥으로 나갔을 때
             if (isIn)
                 _stageTextForm.DrawerModule.Draw(false, DrawDuration / 2, true); 
        }

        public override void OpenView()
        {
            base.OpenView();
            
            BackgroundForm.DoFade(true, FadeDuration, 1);
            if (DrawCurDayText && !_stageTextForm.Text.Contains("0"))
                _stageTextForm.DrawerModule.Draw(true, DrawDuration / 2, true);
        }

        public void StartClose()
        {
            BackgroundForm.DoFade(false, FadeDuration, 1);
            if (DrawNextDayText)
            {
                _stageTextForm.DrawerModule.Draw(true, DrawDuration / 2, true);
                _stageTextForm.TextModule.UpdateText(NextStage);
            }
        }
    }
}