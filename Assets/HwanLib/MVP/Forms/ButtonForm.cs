using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ButtonForm : BaseForm, IInteractable, IPointerDownHandler, IPointerUpHandler
        , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] protected float fadeTime = 0.2f;
        [SerializeField] protected float onHoverAlpha = 0.6f;
        [SerializeField] protected float onClickAlpha = 0.4f;

        public event FormInteracted OnFormInteracted;
        
        private CanvasGroup _canvasGroup;
        public bool Interactive { get; private set; }

        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);
            
            ChildIndex = childIndex;
            
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        
            Interactive = true;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(UIUtil.FadeOut(_canvasGroup, onHoverAlpha, fadeTime));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(UIUtil.FadeIn(_canvasGroup, 1.0f, fadeTime));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _canvasGroup.alpha = onClickAlpha;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1.0f;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnFormInteracted?.Invoke(ChildIndex, UIParamData.UIClickParam);
        }
        
        public void SetInteractive(bool value)
        {
            if (value == false)
                _canvasGroup.alpha = onClickAlpha;
            else
                _canvasGroup.alpha = 1;
            
            Interactive = value;
            _canvasGroup.blocksRaycasts = value;
        }
    }
}
