using HwanLib.MVP.System.BaseMVP.Form;

namespace HwanLib.MVP.System.MVPModule.Form
{
    public abstract class AbstractVisualForm : BaseForm, IUpdatable
    {
        public event UpdateForm OnFormUpdate;
        
        public void UpdateForm()
            => UpdateVisual(OnFormUpdate?.Invoke(ChildIndex));

        protected abstract void UpdateVisual(UIParam data);
    }
}