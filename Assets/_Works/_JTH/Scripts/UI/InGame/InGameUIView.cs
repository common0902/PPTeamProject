using System.Collections.Generic;
using GameLib.PoolObject.Runtime;
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
        private PoolManagerSO _poolManager;
        
        private SwapForm _weaponField;
        private AccessForm _sprintSkill;
        private AccessForm _redMarkBoard;

        private RectTransform[] _redMarks;
        
        public override void InitializeView(GameObject root, List<FormData> formDataList, FormInteracted formInteractedHandler,
            UpdateForm updateFormHandler)
        {
            base.InitializeView(root, formDataList, formInteractedHandler, updateFormHandler);

            _weaponField = GetForm<SwapForm>((int)InGameUIEnum.WeaponField);
            _sprintSkill = GetForm<AccessForm>((int)InGameUIEnum.SprintSkill);
            _redMarkBoard = GetForm<AccessForm>((int)InGameUIEnum.RedMarkBoard);
            
            GetForm<GaugeForm>((int)InGameUIEnum.HpGauge).InitGaugeForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.SprintCover).InitCooldownForm(GaugeType.PosY);
            GetForm<CooldownForm>((int)InGameUIEnum.TopViewCover).InitCooldownForm(GaugeType.PosY);
        }

        public void InitInGameView(RectTransform[] redMarks)
        {
            _redMarks = redMarks;
            
            foreach (var redMark in _redMarks)
            {
                redMark.gameObject.SetActive(false);
                redMark.SetParent(_redMarkBoard.transform);
            }
        }

        public void OnViewChange(bool isTopView)
        {
            if (isTopView == true)
            {
                _weaponField.gameObject.SetActive(false);
                _sprintSkill.gameObject.SetActive(false);
            }
            else
            {
                _weaponField.gameObject.SetActive(true);
                _sprintSkill.gameObject.SetActive(true);
                
                foreach (var redMark in _redMarks)
                    redMark.gameObject.SetActive(false);
            }
        }

        public void SetRedMark(Vector2[] sabotageRedMarkScreenPos)
        {
            Camera cam = Camera.main;
            
            for (int i = 0; i < _redMarks.Length; ++i)
            {
                if (sabotageRedMarkScreenPos.Length <= i)
                    return;
                _redMarks[i].position = sabotageRedMarkScreenPos[i];
                _redMarks[i].gameObject.SetActive(true);
            }
        }
    }
}