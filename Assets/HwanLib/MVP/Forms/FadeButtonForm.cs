using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeButtonForm : AbstractClickForm, IPointerDownHandler, IPointerUpHandler
        , IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected float fadeTime = 0.2f;
        [SerializeField] protected float onHoverAlpha = 0.6f;
        [SerializeField] protected float onClickAlpha = 0.4f;

        private CanvasGroup _canvasGroup;
        public bool Interactive { get; private set; }

        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        
            Interactive = true;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_canvasGroup.interactable == false) return;
            
            StopAllCoroutines();
            StartCoroutine(UIUtil.FadeOut(_canvasGroup, onHoverAlpha, fadeTime));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_canvasGroup.interactable == false) return;
            
            StopAllCoroutines();
            StartCoroutine(UIUtil.FadeIn(_canvasGroup, 1.0f, fadeTime));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_canvasGroup.interactable == false) return;
            
            _canvasGroup.alpha = onClickAlpha;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_canvasGroup.interactable == false) return;
            
            _canvasGroup.alpha = 1.0f;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_canvasGroup.interactable == false) return;
            
            base.OnPointerClick(eventData);
        }

        public void SetInteractive(bool value)
        {
            if (value == false)
                _canvasGroup.alpha = onClickAlpha;
            else
                _canvasGroup.alpha = 1;
            
            Interactive = value;
            _canvasGroup.interactable = value;
        }
    }
}
