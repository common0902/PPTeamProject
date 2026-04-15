using System.Collections.Generic;
using System.Linq;
using HwanLib.MVP.System.AddFormComponent;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BaseView
    {
        public InteractiveObject OnInteractiveObject;

        public void InitializeView(GameObject root, MVPDataSO dataSO)
        {
            Transform[] children = root.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (dataSO.GetFormDataKeys().Contains(child.name))
                {
                    BaseForm form = child.gameObject
                        .AddFormComponent(dataSO.GetFormData(child.name).formTypeName);
                    form.InitializeForm(OnInteractiveObject);
                }
            }
        }
    }
}