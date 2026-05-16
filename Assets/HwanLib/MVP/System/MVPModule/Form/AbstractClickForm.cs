using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine.EventSystems;

namespace HwanLib.MVP.System.MVPModule.Form
{
    public abstract class AbstractClickForm : BaseForm, IInteractable, IPointerClickHandler
    {
        public event FormInteracted OnFormInteracted;
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnFormInteracted?.Invoke(ChildIndex, UIParamContainer.UIClickParam);
        }
    }
}