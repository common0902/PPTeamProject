using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects.Sabotage;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.UI;

namespace _Works._JTH.Scripts.UI.Tooltip
{
    public class TooltipUIPresenter : BasePresenter
    {
        [SerializeField] private EventChannelSO sabotageChannel;
        
        private TooltipUIView _tooltipView;
        private TooltipUIModel _tooltipModel;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
                        
            _tooltipView = View as TooltipUIView;
            _tooltipModel = Model as TooltipUIModel;
            
            sabotageChannel?.AddListener<FocusedSabotageEvent>(ShowTooltip);
        }
        
        protected override void OnDestroy()
        {
            sabotageChannel?.RemoveListener<FocusedSabotageEvent>(ShowTooltip);
            base.OnDestroy();
        }
        
        private void ShowTooltip(FocusedSabotageEvent eventData)
        {
            if (eventData.IsFocused == false)
            {
                CloseTooltip();
                return;
            }
            
            Sabotage sabotage = eventData.Sabotage;
            _tooltipModel.SetText(sabotage.SabotageData.SabotageName, 
                sabotage.SabotageData.SabotageDesc);

            Vector2 tooltipPos = Camera.main.WorldToScreenPoint(sabotage.transform.position);
            _tooltipView.OpenView();
            _tooltipView.SetSize();
            _tooltipView.SetPosition(tooltipPos);
        }

        private void CloseTooltip()
            => _tooltipView.CloseView();
    }
}