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
        [SerializeField] private int transitionCount;
        [SerializeField] private float transitionTime;
        [SerializeField] private Color minValue;
        [SerializeField] private Color maxValue;
        private Volume _volume;
        private Vignette _vignette;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _vignette);
        }

        [ContextMenu("Emergency")]
        public void StartEmergency()
        {
            StartCoroutine(StartEmergencyCoroutine());
        }
        
        private IEnumerator StartEmergencyCoroutine()
        {
            for (int i = 1; i <= transitionCount; ++i)
            {
                if (i % 2 == 1)
                    yield return StartCoroutine(FadeIn());
                else
                    yield return StartCoroutine(FadeOut());
            }
        }

        private IEnumerator FadeIn()
        {
            Debug.Log("Fade In");
            float t = 0;
            float elapsed;
            while (t < transitionTime)
            {
                t += Time.unscaledDeltaTime;
                elapsed = t / transitionTime;
                Color color = Color.Lerp(minValue, maxValue, elapsed);
                _vignette.color.value = color;
                yield return null;
            }
        }

        private IEnumerator FadeOut()
        {
            Debug.Log("Fade Out");
            float t = 0;
            float elapsed;
            while (t < transitionTime)
            {
                t += Time.unscaledDeltaTime;
                elapsed = t / transitionTime;
                _vignette.color.value = Color.Lerp(maxValue, minValue, elapsed);
                yield return null;
            }
        }

        private void OnValidate()
        {
            if (transitionCount % 2 == 1)
            {
                transitionCount = 0;
                Debug.LogError("트랜지션 횟수는 짝수여야함");
            }
            
        }
    }
}