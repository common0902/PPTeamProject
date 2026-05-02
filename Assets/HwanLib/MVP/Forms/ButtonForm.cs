using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.UIData;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HwanLib.MVP.Forms
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ButtonForm : IBaseForm, IPointerDownHandler, IPointerUpHandler
        , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] protected float fadeTime = 0.2f;
        [SerializeField] protected float onHoverAlpha = 0.6f;
        [SerializeField] protected float onClickAlpha = 0.4f;
        
        private CanvasGroup _canvasGroup;
        protected bool Interactive;

        public override void InitializeForm(int childIndex)
        {
            base.InitializeForm(childIndex);
            
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            Interactive = true;
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(UIUtil.FadeOut(_canvasGroup, onHoverAlpha, fadeTime));
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(UIUtil.FadeIn(_canvasGroup, 1.0f, fadeTime));
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _canvasGroup.alpha = onClickAlpha;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1.0f;
        }
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnInteract(UIParamData.UIClickParam);
        }
        
        public virtual void SetInteractive(bool value)
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
