using System.Collections.Generic;
using DG.Tweening;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class SwapForm : AbstractVisualForm
    {
        [SerializeField] private float swapDuration = 0.3f;

        private Dictionary<int, int> _childDict;
        private RectTransform[] _currentChildren;
        private Sequence _sequence;
        private float _startSwapTime;

        private void Awake()
        {
            _childDict = new Dictionary<int, int>();
            _currentChildren = new RectTransform[transform.childCount];
            
            for (int i = 0; i < transform.childCount; ++i)
            {
                RectTransform rectTrm = transform.GetChild(i).GetComponent<RectTransform>();
                // rectTrm.gameObject.GetOrAddComponent<LayoutElement>().ignoreLayout = true;
                _childDict.Add(i, i);
                _currentChildren[i] = rectTrm;
            }
            
            // LayoutGroup은 위치랑 모양만 잡고 끄기
            SetOffLayoutGroup();
            _sequence = DOTween.Sequence();
        }
        
        private void SetOffLayoutGroup()
        {
            VerticalLayoutGroup layoutGroup = GetComponent<VerticalLayoutGroup>();
            if (layoutGroup == null || layoutGroup.enabled == false)
                return;
            
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();            
            
            layoutGroup.enabled = false;
        }

        protected override void UpdateVisual(UIParam data)
        {
            UISwapParam swapData = (UISwapParam)data;
            
            SwapItem(swapData.ItemEnum, swapData.TargetIndex);
        }

        private void SwapItem(int itemEnum, int targetIndex)
        {
            int currentItemIdx = _childDict[itemEnum];
            if (currentItemIdx == targetIndex)
                return;

            if (!_sequence.IsActive())
                _sequence.timeScale = 1;
            else
            {
                // 남은 시간 / 원하는 시간을 timeScale에 곱하면 남은 시간이 원하는 시간동안 흐름
                // 남은 시간은 swapDuration - (Time.time - _startSwapTime)으로 구할 수 있다.
                float remainTime = swapDuration - (Time.time - _startSwapTime);
                //남은 시간과 추가된 시간을 timeScale로 나눠서 앞으로 몇초가 흘러야 움직임이 끝날지 구하고, 그걸 원하는 시간으로 나눠서 곱하기
                _sequence.timeScale *= (swapDuration + remainTime) / _sequence.timeScale / swapDuration;
            }
            
            _startSwapTime = Time.time;

            Vector2 currentPos = _currentChildren[currentItemIdx].anchoredPosition;
            Vector2 targetPos = _currentChildren[targetIndex].anchoredPosition;

            _sequence = DOTween.Sequence();
            _sequence.Append(_currentChildren[currentItemIdx]
                    .DOAnchorPos(currentPos, swapDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true))
                .Join(_currentChildren[targetIndex]
                    .DOAnchorPos(targetPos, swapDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true));

            //Swap
            (_currentChildren[targetIndex], _currentChildren[currentItemIdx]) 
                = (_currentChildren[currentItemIdx], _currentChildren[targetIndex]);
        }
        
        private void OnDestroy()
        {
            _sequence.Complete();
            _sequence.Kill();
        }
    }
}