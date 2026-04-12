using UnityEngine;

namespace MVP.System
{
    public abstract class BaseView
    {
        public InteractiveObject OnInteractiveObject;

        public void InitializeView(GameObject root)
        {
            BaseForm[] objects = root.GetComponentsInChildren<BaseForm>();
            foreach (BaseForm obj in objects)
            {
                // obj.Initialize(OnInteractiveObject);
            }
        }
    }
}