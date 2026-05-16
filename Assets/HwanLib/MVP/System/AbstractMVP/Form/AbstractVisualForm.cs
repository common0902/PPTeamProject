using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;

namespace HwanLib.MVP.System.AbstractMVP.Form
{
    public abstract class AbstractVisualForm : BaseForm, IUpdatable
    {
        public event UpdateForm OnFormUpdate;
        
        public void UpdateForm()
            => UpdateVisual(OnFormUpdate?.Invoke(ChildIndex));

        protected abstract void UpdateVisual(UIParam data);
    }
}