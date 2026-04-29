using System;
using HwanLib.MVP.System.BaseMVP;
using UnityEngine;

namespace HwanLib.MVP.System.GenerateUI
{
    public class UIGenerator : MonoBehaviour
    {
        [SerializeField] private MVPDataListSO mvpDataList;

        private void Awake()
        {
            GenerateAllUI();
        }

        private void GenerateAllUI()
        {
            foreach (MVPDataSO dataSO in mvpDataList.mvpData)
            {
                BasePresenter presenter = Instantiate(dataSO.parentPrefab, transform);
                presenter.InitializePresenter(dataSO);
            }
        }
    }
}