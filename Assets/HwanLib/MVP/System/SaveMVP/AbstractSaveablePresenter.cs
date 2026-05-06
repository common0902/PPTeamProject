using _Script.SaveSystem;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.GenerateUI;
using UnityEngine;

namespace HwanLib.MVP.System.SaveMVP
{
    public abstract class AbstractSaveablePresenter : BasePresenter, IRestorable, IStorable
    {
        [field: SerializeField] public SaveData SaveId { get; private set; }
        
        protected new AbstractSaveableModel Model;

        public override void InitializePresenter(MVPDataSO dataSO)
        {
            if (!dataSO.GetModelType().IsSubclassOf(typeof(AbstractSaveableModel)))
            {
                Debug.LogWarning("Model이 AbstractSaveableModel를 상속 받지 않았습니다.");
                return;
            }
            base.InitializePresenter(dataSO);
            
            Model = base.Model as AbstractSaveableModel;
            Model.SetDefaultData();
        }

        public string StoreData()
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