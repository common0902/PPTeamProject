using System;
using System.Collections.Generic;
using HwanLib.MVP.System.BaseMVP;
using UnityEngine;

namespace HwanLib.MVP.System.GenerateUI
{
    public enum MultiableUIState
    {
        None,
        Opening,
        Opened,
    }
    
    public class MultiableUIs
    {
        public MultiableUIState State { get; set; }
        public IMultiable EndPresenter { get; set; }
    }
    
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MVPDataListSO mvpDataList;

        private Dictionary<Type, MultiableUIs> _multiableUIDict;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _multiableUIDict = new Dictionary<Type, MultiableUIs>();
            GenerateAllUI();
        }

        private void GenerateAllUI()
        {
            foreach (MVPDataSO singleUI in mvpDataList.singleUIData)
            {
                BasePresenter presenter = Instantiate(singleUI.parentPrefab, transform);
                Debug.Assert(presenter is not IMultiable
                    , $"singleUIData에 multiable UI가 존재합니다. {presenter.gameObject.name}");
                presenter.InitializePresenter(singleUI.GetFormDataList(), 
                    singleUI.GetViewType(), singleUI.GetModelType());
            }

            foreach (MultiableUIData multiableData in mvpDataList.multiableUIData)
            {
                MVPDataSO dataSO = multiableData.MvpDataSO;
                
                MultiableUIs multiableUIs = new MultiableUIs() { State = MultiableUIState.None };
                _multiableUIDict.Add(dataSO.parentPrefab.GetType(), multiableUIs);
                
                for (int i = 0; i < multiableData.Count; ++i)
                {
                    AddMultiableUI(dataSO);
                }
            }
        }

        private void AddMultiableUI(MVPDataSO dataSO)
        {
            BasePresenter presenter = Instantiate(dataSO.parentPrefab, transform);
            var multiableData = _multiableUIDict[presenter.GetType()];
            
            presenter.InitializePresenter(dataSO.GetFormDataList(), dataSO.GetViewType(), dataSO.GetModelType());
                    
            IMultiable multiableUI = presenter as IMultiable;
            Debug.Assert(multiableUI != null,
                $"multiable UI Data는 IMultiable을 상속해야 합니다. gameObject: {presenter.gameObject.name}");
                    
            multiableUI.TryOpen += OpenMultiableUI;
            multiableData.EndPresenter = multiableUI;
        }
        
        private bool OpenMultiableUI(IMultiable presenter)
        {
            Type presenterType = presenter.GetType();
            MultiableUIs multiableUIs = _multiableUIDict[presenterType];
            
            //Open 가능, 열어야 함.
            switch (multiableUIs.State)
            {
                case MultiableUIState.None:
                    multiableUIs.State = MultiableUIState.Opening;
                    break;
                case MultiableUIState.Opened:
                    if (presenter == multiableUIs.EndPresenter)
                        multiableUIs.State = MultiableUIState.None;
                    
                    return false;
            }

            if (presenter.CanOpen == false)
                return false;
            
            if (presenter != multiableUIs.EndPresenter)
            {
                presenter.OpenUI();
                multiableUIs.State = MultiableUIState.Opened;
            }
            //Open 불가능, 열어야함, 마지막
            else
            {
                AddMultiableUI(mvpDataList.GetDataFromMultiable(presenterType));
                multiableUIs.EndPresenter.OpenUI();
                multiableUIs.State = MultiableUIState.None;
            }

            return true;
        }
    }
}