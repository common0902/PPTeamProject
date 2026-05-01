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

        //디폴트 매개 변수가 null이고, null을 받을 시엔 프레젠터에서 업데이트 로직에 해당하는 메서드(매개 변수가 없음)가 있다면 그걸로 넘김
        public void UpdateVisual()
            => SetVisual(OnFormInteracted?.Invoke(_childIndex));

        protected virtual void SetVisual(ChangedData data)
        {
            // SceneChange 같은 Visual이 없는 Form이 있을 수 있기 때문에 비워놓는다.
        }

        protected void OnInteractive(ChangedData changedValue) 
            => SetVisual(OnFormInteracted?.Invoke(_childIndex, changedValue));
    }
}