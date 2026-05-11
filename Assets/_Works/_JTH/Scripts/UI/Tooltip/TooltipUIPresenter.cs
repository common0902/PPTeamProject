using System;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;
using UnityEngine.InputSystem;

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

#if UNITY_EDITOR
        
        private void Update()
        {
            if (!Keyboard.current.ctrlKey.isPressed)
                return;
            
            if (Keyboard.current.tKey.wasPressedThisFrame)
                TestTooltip();
            if (Keyboard.current.gKey.wasPressedThisFrame)
                CloseTooltip();
        }
        
        public void TestTooltip()
        {
            _tooltipModel.SetText("가스 누출", 
                "가스를 누출시킵니다.\n\n누출된 가스에 닿는 오브젝트는\n<color=blue>둔화</color>와 <color=yellow>실명</color> 효과가 적용됩니다. ");
            Vector2 tooltipPos = Mouse.current.position.ReadValue();
            _tooltipView.OpenView(tooltipPos);
        }
#endif
        
        private void ShowTooltip(FocusedSabotageEvent eventData)
        {
            if (eventData.IsFocused == false)
            {
                CloseTooltip();
                return;
            }
            
            _tooltipModel.SetText(eventData.Sabotage.SabotageData.SabotageName, 
                eventData.Sabotage.SabotageData.SabotageDesc);
            Vector2 tooltipPos = Camera.main.WorldToScreenPoint(eventData.Sabotage.transform.position);
            _tooltipView.OpenView(tooltipPos);
        }

        private void CloseTooltip()
            => _tooltipView.CloseTooltip();
    }
}