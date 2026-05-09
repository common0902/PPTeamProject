using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Works._CJW.Scripts.Rendering
{
    public class EmergencyEvent : MonoBehaviour
    {
        [SerializeField] private float transitionTime;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color healColor;
        [SerializeField] private Color damageColor;
        private Volume _volume;
        private Vignette _vignette;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _vignette);
        }

        [ContextMenu("Test")]
        private void Test()
        {
            StartCoroutine(Transition(healColor, 0.35f));
        }

        public void SirenTransition()
        {
            StartCoroutine(Transition(damageColor, 0.35f));
        }
        
        private IEnumerator Transition(Color changeColor, float maxValue)
        {
            float t = 0;
            _vignette.color.value = changeColor;
            while (t < transitionTime)
            {
                float elapsed = t / transitionTime;
                float value = Mathf.Sin(elapsed * Mathf.PI) * maxValue; // 0 ~ 1 ~ 0
                Debug.Log(value);
                _vignette.intensity.value = value;
                t += Time.deltaTime;
                yield return null;
            }

            _vignette.color.value = defaultColor;
        }
    }
}