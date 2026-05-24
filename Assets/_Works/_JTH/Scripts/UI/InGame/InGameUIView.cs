using System.Collections.Generic;
using HwanLib.MVP.Forms;
using HwanLib.MVP.Forms.Module.Gauge;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace _Works._JTH.Scripts.UI.InGame
{
    public class InGameUIView : BaseView
    {
        private SwapForm _weaponField;
        private AccessForm _sprintSkill;
        private AccessForm _redMarkBoard;
        private AccessForm _crossHair;

        private List<RectTransform> _redMarkList;
        
        public int GetRedMarkCount => _redMarkList.Count;
        
        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);

            _weaponField = GetForm<SwapForm>((int)InGameUIEnum.WeaponField);
            _sprintSkill = GetForm<AccessForm>((int)InGameUIEnum.SprintSkill);
            _redMarkBoard = GetForm<AccessForm>((int)InGameUIEnum.RedMarkBoard);
            _crossHair = GetForm<AccessForm>((int)InGameUIEnum.CrossHair);
            
            GetForm<GaugeForm>((int)InGameUIEnum.HpGauge).InitGaugeForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.SprintCover).InitCooldownForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.TopViewCover).InitCooldownForm(GaugeType.PosY);

            _redMarkList = new List<RectTransform>();
        }

        public void AddRedMark(RectTransform redMark)
        {
            _redMarkList.Add(redMark);
            redMark.gameObject.SetActive(false);
            redMark.SetParent(_redMarkBoard.transform);
        }

        public void OnViewChange(bool isTopView)
        {
            if (isTopView == true)
            {
                _weaponField.gameObject.SetActive(false);
                _sprintSkill.gameObject.SetActive(false);
                _crossHair.gameObject.SetActive(false);
            }
            else
            {
                _weaponField.gameObject.SetActive(true);
                _sprintSkill.gameObject.SetActive(true);
                _crossHair.gameObject.SetActive(true);
                
                foreach (var redMark in _redMarkList)
                    redMark.gameObject.SetActive(false);
            }
        }

        public void SetRedMark(List<Vector2> redMarkScreenPosList)
        {
            for (int i = 0; i < redMarkScreenPosList.Count; ++i)
            {
                _redMarkList[i].position = redMarkScreenPosList[i];
                _redMarkList[i].gameObject.SetActive(true);
            }
        }
    }
}