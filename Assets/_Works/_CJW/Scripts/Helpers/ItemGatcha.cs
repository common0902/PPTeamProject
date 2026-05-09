using System.Collections.Generic;
using _Works._CJW.Scripts.Objects.Box;
using NUnit.Framework;
using UnityEngine;

namespace _Works._CJW.Scripts.Helpers
{
    public class ItemGatcha
    {
        private List<ItemDataSO> _percents;
        public ItemGatcha(List<ItemDataSO> percents)
        {
            _percents = percents;
        }
        
        public ItemDataSO GetRandomItem()
        {
            float totalPercent = 0;
            foreach (ItemDataSO dataSo in _percents)
            {
                totalPercent += dataSo.dropPercent;
            }
            float randomValue = Random.Range(0f, totalPercent);
            float cumulativePercent = 0f;

            foreach (var item in _percents)
            {
                cumulativePercent += item.dropPercent;
                if (randomValue <= cumulativePercent)
                {
                    return item;
                }
            }

            return null; // 확률이 100%가 되도록 설정되어 있다면 이 부분은 실행되지 않아야 합니다.
        }
    }
}