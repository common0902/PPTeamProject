using System.Collections.Generic;
using HwanLib.MVP.System.AddFormComponent;
using UnityEngine;

namespace HwanLib.MVP.System
{
    public abstract class BaseView
    {
        public InteractObject OnInteractObject;
        public List<BaseForm> Forms { get; private set; }

        public void InitializeView(GameObject root, List<FormData> formDataList)
        {
            Forms = new List<BaseForm>();
            
            Transform[] children = root.GetComponentsInChildren<Transform>();
            for (var i = 0; i < formDataList.Count; i++)
            {
                Transform child = children[formDataList[i].childIndex];
                
                BaseForm form = child.gameObject
                    .AddFormComponent(formDataList[i].formTypeName);
                form.InitializeForm(OnInteractObject);
                form.UpdateVisual();
                Forms.Add(form);
            }
        }
    }
}