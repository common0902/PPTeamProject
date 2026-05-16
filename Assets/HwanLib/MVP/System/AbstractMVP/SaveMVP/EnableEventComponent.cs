using System;
using UnityEngine;

namespace HwanLib.MVP.System.AbstractMVP.SaveMVP
{
    public class EnableEventComponent : MonoBehaviour
    {
        public event Action<bool> OnEnabled;

        private void OnEnable()
        {
            OnEnabled?.Invoke(true);
        }

        private void OnDisable()
        {
            OnEnabled?.Invoke(false);
        }
    }
}