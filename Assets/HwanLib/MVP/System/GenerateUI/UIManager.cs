using System;
using System.Collections.Generic;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Multiable;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

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
        public IMultiablePresenter EndPresenter { get; set; }
    }
    
    public class UIManager : LightSingleton<UIManager>
    {
        // [SerializeField] private Input
        [SerializeField] private MVPDataListSO mvpDataList;
        [SerializeField] private EventSystem eventSystemPrefab;
        [SerializeField] private WorldUICamera worldUICamera;
        [SerializeField] private LayerMask uiLayer = 1 << 5;
 
        private Dictionary<Type, MultiableUIs> _multiableUIDict;
        private List<BasePresenter> _presenterList;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (FindAnyObjectByType<EventSystem>() == null)
                Instantiate(eventSystemPrefab, transform);
            Instantiate(worldUICamera, Camera.main.transform);

            _multiableUIDict = new Dictionary<Type, MultiableUIs>();
            _presenterList = new List<BasePresenter>();
            
            GenerateAllUI();
            SetOverlyCamera();
        }

        private void GenerateAllUI()
        {
            foreach (MVPDataSO singleUI in mvpDataList.singleUIData)
            {
                BasePresenter presenter = Instantiate(singleUI.parentPrefab, transform);
                Debug.Assert(presenter is not IMultiablePresenter
                    , $"singleUIData에 multiable UI가 존재합니다. {presenter.gameObject.name}");
                presenter.InitializePresenter(singleUI.GetFormDataList(), 
                    singleUI.GetViewType(), singleUI.GetModelType());
                
                _presenterList.Add(presenter);
            }

            foreach (MultiableUIData multiableData in mvpDataList.multiableUIData)
            {
                MVPDataSO dataSO = multiableData.MvpDataSO;
                
                MultiableUIs multiableUIs = new MultiableUIs { State = MultiableUIState.None };
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
                    
            IMultiablePresenter multiablePresenterUI = presenter as IMultiablePresenter;
            Debug.Assert(multiablePresenterUI != null,
                $"multiable UI Data는 IMultiable을 상속해야 합니다. gameObject: {presenter.gameObject.name}");
                    
            multiablePresenterUI.TryOpen += OpenMultiableUI;
            multiableData.EndPresenter = multiablePresenterUI;
            
            _presenterList.Add(presenter);
        }
        
        private bool OpenMultiableUI(IMultiablePresenter presenter)
        {
            Type presenterType = presenter.GetType();
            MultiableUIs multiableUIs = _multiableUIDict[presenterType];
            
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

            //열어야 함.
            //열 수 있음
            if (presenter.MultiableView.CanOpen)
            {
                presenter.OpenUI();
                multiableUIs.State = MultiableUIState.Opened;
            }
            //열 수 없음, 마지막,
            else if (presenter == multiableUIs.EndPresenter)
            {
                AddMultiableUI(mvpDataList.GetDataFromMultiable(presenterType));
                multiableUIs.EndPresenter.OpenUI();
                multiableUIs.State = MultiableUIState.None;
            }
            //열 수 없음
            else
                return false;

            return true;
        }

        private void SetOverlyCamera()
        {
            var cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
            
            foreach (BasePresenter presenter in _presenterList)
            {
                if (presenter.IsWorldPosition)
                {
                    Camera overlayCam = cameraData.cameraStack
                        .Find(cam => cam.cullingMask == uiLayer);
                    
                    presenter.transform.GetChild(0).GetComponent<Canvas>().worldCamera = overlayCam;
                }
            }
        }
    }
}
