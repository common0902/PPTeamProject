using System;
using System.Collections.Generic;
using _Script.Agent.Modules;
using _Works._JYG._Script.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace _Works._JYG._Script.UI
{
    public class CautionBar : MonoBehaviour, IModule
    {
        private AbstractEnemy _enemy;
        [SerializeField] private Image bar;
        [SerializeField] private Gradient cautionGradient;
        public void Initialize(ModuleOwner moduleOwner)
        {
            _enemy = moduleOwner as AbstractEnemy;
        }

        private void LateUpdate()
        {
            float caution = _enemy.GetEnemyCaution;
            bar.fillAmount = caution;
            bar.color = cautionGradient.Evaluate(caution);
            
            if (caution <= 0 || caution >= 1)    //Caution이 0이나 1이 되면 사라진다.
            {
                bar.gameObject.SetActive(false);
            }
            else if ((caution > 0  || caution < 1) && !bar.gameObject.activeSelf) //0이나 1이 아닐때 나타난다.
            {
                bar.gameObject.SetActive(true);
            }
        }
    }
}
