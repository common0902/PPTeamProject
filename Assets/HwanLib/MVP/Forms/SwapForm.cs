using System.Collections.Generic;
using DG.Tweening;
using HwanLib.MVP.System;
using HwanLib.MVP.System.AbstractMVP.Form;
using HwanLib.MVP.System.BaseMVP;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine;
using UnityEngine.UI;

namespace HwanLib.MVP.Forms
{
    public class SwapForm : AbstractVisualForm, IInitializable
    {
        [SerializeField] private float swapDuration = 0.3f;

        private Dictionary<int, int> _childDict;
        private RectTransform[] _currentChildren;
        private Sequence _sequence;
        private float _startSwapTime;

        public void Initialize()
        {
            _childDict = new Dictionary<int, int>();
            _currentChildren = new RectTransform[transform.childCount];
            
            for (int i = 0; i < transform.childCount; ++i)
            {
                RectTransform rectTrm = transform.GetChild(i).GetComponent<RectTransform>();
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
            int targetItemIdx = _childDict[itemEnum];
            if (targetItemIdx == targetIndex)
                return;

            DoMove(_sequence, targetItemIdx, targetIndex);

            //Swap
            (_currentChildren[targetIndex], _currentChildren[targetItemIdx]) 
                = (_currentChildren[targetItemIdx], _currentChildren[targetIndex]);
            (_childDict[targetIndex], _childDict[targetItemIdx]) 
                = (_childDict[targetItemIdx], _childDict[targetIndex]);
        }

        private void DoMove(Sequence seq, int targetItemIdx, int targetIndex)
        {
            if (_sequence.IsActive() == true)
            {
                _sequence.Complete();
                _sequence.Kill();
            }
                
            Vector2 currentPos = _currentChildren[targetItemIdx].anchoredPosition;
            Vector2 targetPos = _currentChildren[targetIndex].anchoredPosition;
            
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_currentChildren[targetItemIdx]
                    .DOAnchorPos(targetPos, swapDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true))
                .Join(_currentChildren[targetIndex]
                    .DOAnchorPos(currentPos, swapDuration)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true));
        }
        
        private void OnDestroy()
        {
            if (_sequence.IsActive() == true)
            {
                _sequence.Complete();
                _sequence.Kill();
            }
        }
    }
}