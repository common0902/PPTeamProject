using UnityEngine;

namespace HwanLib.MVP.System.BaseMVP.Form
{
    public abstract class BaseForm : MonoBehaviour
    {
        protected int ChildIndex;

        public virtual void InitializeForm(int childIndex)
        {
            ChildIndex = childIndex;
        }
    }
}