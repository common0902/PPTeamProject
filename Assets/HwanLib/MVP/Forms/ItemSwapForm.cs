using DG.Tweening;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class ItemSwapForm : AbstractVisualForm
    {
        [SerializeField] private float swapDuration;
        
        private RectTransform[] _children;
        private Sequence _sequence;
        private float _startSwapTime;

        private void Awake()
        {
            _children = new RectTransform[transform.childCount];
            for (int i = 0; i < transform.childCount; ++i)
            {
                _children[i] = transform.GetChild(i).GetComponent<RectTransform>();
            }
            
            // LayoutGroup은 위치랑 모양만 잡고 끄기
            GetComponent<LayoutGroup>().enabled = false;
            _sequence = DOTween.Sequence();
        }

        protected override void UpdateVisual(UIParam data)
        {
            UISwapParam swapData = (UISwapParam)data;
            
            SwapItem(swapData.Item1, swapData.Item2);
        }

        public void SwapItem(int item1Idx, int item2Idx)
        {
            if (item1Idx == item2Idx)
                return;
            

            if (!_sequence.IsActive())
                _startSwapTime = Time.time;
            else
            {
                // 남은 시간 / 원하는 시간을 timeScale에 곱하면 남은 시간이 원하는 시간만큼 흐름
                // 남은 시간은 swapDuration - (Time.time - _startSwapTime)으로 구할 수 있다.
                float remainTime = swapDuration - (Time.time - _startSwapTime);
                //남은 시간과 추가된 시간을 timeScale로 나눠서 앞으로 몇초가 흘러야 움직임이 끝날지 구하고, 그걸 원하는 시간으로 나눠서 곱하기
                _sequence.timeScale *= (swapDuration + remainTime) / _sequence.timeScale / remainTime;
            }


            for (int i = 0; i < 2; ++i)
            {
                int idx1 = i == 0 ? 0 : 1, idx2 = i != 0 ? 0 : 1;
                _sequence.Append(_children[idx1].DOMove(_children[idx2].anchoredPosition, swapDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true));
            }

            //Swap
            (_children[item1Idx], _children[item2Idx]) 
                = (_children[item2Idx], _children[item1Idx]);
        }
        
        private void OnDestroy()
        {
            _sequence.Complete();
            _sequence.Kill();
        }
    }
}