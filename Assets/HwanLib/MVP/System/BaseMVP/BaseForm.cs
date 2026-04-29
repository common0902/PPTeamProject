using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP
{
    public abstract class BaseForm : MonoBehaviour
    {
        public event FormInteracted OnFormInteracted;
        private int _childIndex;

        public virtual void InitializeForm(int childIndex)
        {
            _childIndex = childIndex;
        }

        public void UpdateVisual() => OnInteractive(null);

        protected virtual void SetVisual(ChangedData data)
        {
            // SceneChange 같은 Visual이 없는 Form이 있을 수 있기 때문에 비워놓는다.
        }

        protected void OnInteractive(ChangedData changedValue) 
        {
            SetVisual(OnFormInteracted?.Invoke(_childIndex, changedValue));
        }
    }
}