using System;
using System.Collections.Generic;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;

namespace _Works._JTH.Scripts.UI.InteractiveBtn
{
    public class InteractiveBtnPresenter : BasePresenter
    {
        public override bool IsWorldPosition => true;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            base.InitializePresenter(formData, viewType, modelType);
            
            
        }
    }
}