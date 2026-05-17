using System;
using System.Collections.Generic;
using _Script.SaveSystem;
using _Script.ScriptableObject.Event;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.AbstractMVP.SaveMVP
{
    public abstract class AbstractSaveablePresenter : BasePresenter, IRestorable, IStorable
    {
        [field: SerializeField] public SaveData SaveId { get; private set; }
        [SerializeField] protected EventChannelSO saveChannel;
        
        protected new ISaveableModel Model;
        private EnableEventComponent _enableEventCompo;

        public override void InitializePresenter(List<FormData> formData, Type viewType, Type modelType)
        {
            if (!typeof(ISaveableModel).IsAssignableFrom(modelType))
            {
                Debug.LogWarning("Model이 ISaveableModel를 상속 받지 않았습니다.");
                return;
            }
                
            base.InitializePresenter(formData, viewType, modelType);
            
            Model = (ISaveableModel)base.Model;
            Model.SetDefaultValue();
            
            _enableEventCompo = View.RootCanvas.gameObject.AddComponent<EnableEventComponent>();
            _enableEventCompo.OnEnabled += EnableHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _enableEventCompo.OnEnabled -= EnableHandler;
        }

        private void EnableHandler(bool isEnabled)
        {
            if (isEnabled)
                saveChannel.RaiseEvent(SaveEvents.RestoreDataEvent);
        }
        
        public virtual string StoreData()
        {
            return Model.StoreData();
        }

        public void RestoreData(string data)
        {
            Model.RestoreData(data);
            View.UpdateView();
        }
    }
}