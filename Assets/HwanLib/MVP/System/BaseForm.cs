using System.Linq;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BaseForm : MonoBehaviour
    {
        private InteractObject _onInteractObject;

        public virtual void InitializeForm(InteractObject onInteractObject)
        {
            _onInteractObject = onInteractObject;
        }

        protected virtual void SetVisual<TUseData>(TUseData data) where TUseData : ChangedData
        {
            // SceneChange 같은 Visual이 없는 Form이 있을 수 있기 때문에 비워놓는다.
        }

        public void UpdateVisual() => OnInteractive<ChangedData>(null);

        /// <summary>
        /// Model에 접근
        /// </summary>
        protected void OnInteractive<TUseData>(TUseData changedValue) where TUseData : ChangedData
        {
            SetVisual(_onInteractObject?.Invoke(gameObject.name, changedValue) as TUseData);
        }

        protected T GetPartOfFormComponent<T>() where T : Component
        {
            return GetComponentsInChildren<T>().Single(compo => compo.name.Contains("f_"));
        }
    }
}