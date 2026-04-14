using System.Collections.Generic;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BaseView
    {
        public InteractiveObject OnInteractiveObject;

        public void InitializeView(GameObject root, List<FormData> formDataList)
        {
            Transform[] children = root.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                foreach (FormData formData in formDataList)
                {
                    if (child.gameObject == formData.formObject)
                    {
                        BaseForm form = formData.addComponentMethod.Invoke(child.gameObject);
                        // child.gameObject.AddComponent()
                    }
                }
            }
        }
    }
}