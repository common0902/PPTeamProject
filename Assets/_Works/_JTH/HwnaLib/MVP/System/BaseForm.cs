using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BaseForm : MonoBehaviour
    {
        public string TargetMethodName { get; private set; }
        
        private readonly InteractiveObject _onInteractiveObject;

        private BaseForm(InteractiveObject onInteractiveObject)
        {
            _onInteractiveObject = onInteractiveObject;
        }

        protected TForm OnInteractive<TForm>(TForm value) where TForm : BaseUIData
        {
            return _onInteractiveObject?.Invoke(TargetMethodName, value) as TForm;
        }

        public abstract BaseForm AddComponentToObject(GameObject childGameObject);
    }
}