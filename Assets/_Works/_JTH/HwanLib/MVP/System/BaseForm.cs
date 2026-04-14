using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BaseForm : MonoBehaviour
    {
        private InteractiveObject _onInteractiveObject;

        private void InitializeForm(InteractiveObject onInteractiveObject)
        {
            _onInteractiveObject = onInteractiveObject;
        }

        protected TForm OnInteractive<TForm>(TForm value) where TForm : BaseUIData
        {
            return _onInteractiveObject?.Invoke(gameObject, value) as TForm;
        } 
    }
}