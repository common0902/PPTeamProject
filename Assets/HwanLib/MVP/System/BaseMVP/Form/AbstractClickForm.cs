using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HwanLib.MVP.System.BaseMVP.Form
{
    public abstract class AbstractClickForm : BaseForm, IInteractable, IPointerClickHandler
    {
        public event FormInteracted OnFormInteracted;
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnFormInteracted?.Invoke(ChildIndex, UIParamData.UIClickParam);
        }
    }
}