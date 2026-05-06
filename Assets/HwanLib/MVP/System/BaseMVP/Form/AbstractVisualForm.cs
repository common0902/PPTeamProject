using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace HwanLib.MVP.System.BaseMVP.Form
{
    public abstract class AbstractVisualForm : BaseForm, IUpdatable
    {
        public event UpdateForm OnFormUpdate;
        
        public void UpdateForm()
            => UpdateVisual(OnFormUpdate?.Invoke(ChildIndex));

        protected abstract void UpdateVisual(UIParam data);
    }
}