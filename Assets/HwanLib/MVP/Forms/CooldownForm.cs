using System.Collections;
using HwanLib.MVP.System;
using HwanLib.MVP.System.BaseMVP.Form;
using HwanLib.MVP.UIData;
using UnityEngine;

namespace HwanLib.MVP.Forms
{
    public class CooldownForm : AbstractVisualForm
    {
        private enum CooldownType
        {
            PosY,
        }
        
        [SerializeField] private CooldownType cooldownType = CooldownType.PosY;

        private AbstractCooldown _cooldown;
        
        protected void InitCooldownForm(float cooldownTime)
        {
            switch (cooldownType)
            {
                case CooldownType.PosY:
                    _cooldown = new PosYCooldown();
                    break;
            }
            
            _cooldown.InitCooldown(gameObject, cooldownTime);
        }

        protected override void UpdateVisual(UIParam data)
        {
            float ratio = ((UIFloatParam)data).Value;
            
            _cooldown.CooldownRatio = ratio;
            if (ratio != 0)
                StartCoroutine(_cooldown.StartCooldown());
        }

        private abstract class AbstractCooldown
        {
            public float CooldownRatio
            {
                get => Mathf.Clamp01(_currentCooldown / _cooldownTime);
                set
                {
                    _currentCooldown = _cooldownTime * Mathf.Clamp01(value);
                    SetCooldown();
                }
            }


            private float _currentCooldown;
            private float _cooldownTime;
            
            public virtual void InitCooldown(GameObject gameObject, float cooldownTime)
            {
                _cooldownTime = cooldownTime;
                CooldownRatio = 0;
            }

            public IEnumerator StartCooldown()
            {
                CooldownRatio = 1;
                while (CooldownRatio != 0)
                {
                    yield return null;
                    _currentCooldown -= Time.deltaTime;
                    SetCooldown();
                }
                CooldownRatio = 0;
            }

            protected abstract void SetCooldown();
        }

        private class PosYCooldown : AbstractCooldown
        {
            private Transform _targetTransform;

            public override void InitCooldown(GameObject gameObject, float cooldownTime)
            {
                base.InitCooldown(gameObject, cooldownTime);
                _targetTransform = gameObject.transform;
            }

            protected override void SetCooldown()
                => _targetTransform.localScale = new Vector3(1, CooldownRatio, 1);
        }
    }
}