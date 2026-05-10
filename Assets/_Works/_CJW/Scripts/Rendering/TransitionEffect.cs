using System;
using System.Collections;
using _Script.ScriptableObject.Event;
using _Works._JYG._Script.EventChannel.SystemEvent;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Works._CJW.Scripts.Rendering
{
    public class TransitionEffect : MonoBehaviour
    {
        [SerializeField] private EventChannelSO playerEventChannel;
        [SerializeField] private float transitionTime;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color healColor;
        [SerializeField] private Color sirenColor;
        [SerializeField] private float sirenIntensity = 0.35f;
        private Volume _volume;
        private Vignette _vignette;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _vignette);
            playerEventChannel.AddListener<SirenCameraEffect>(SirenTransition);
        }

        [ContextMenu("Test")]
        void Test()
        {
            StartCoroutine(Transition(sirenColor, sirenIntensity));
        }
        
        private void SirenTransition(SirenCameraEffect evt)
        {
            StartCoroutine(Transition(sirenColor, sirenIntensity));
        }
        
        private IEnumerator Transition(Color changeColor, float maxValue = 0.35f)
        {
            float t = 0;
            float value;
            _vignette.color.value = changeColor;
            while (t < transitionTime)
            {
                float elapsed = t / transitionTime;
                value = Mathf.Lerp(0, maxValue, elapsed);
                // float value = Mathf.Sin(elapsed * Mathf.PI) * maxValue; // 0 ~ 1 ~ 0
                _vignette.intensity.value = value;
                t += Time.deltaTime;
                yield return null;
            }
            _vignette.intensity.value = maxValue;
            // _vignette.color.value = defaultColor;
        }
        
        private IEnumerator RollbackTransition()
        {
            float t = 0;
            float value;
            float currentValue = _vignette.intensity.value;
            _vignette.color.value = defaultColor;
            while (t < transitionTime)
            {
                float elapsed = t / transitionTime;
                value = Mathf.Lerp(currentValue, 0, elapsed);
                _vignette.intensity.value = value;
                t += Time.deltaTime;
                yield return null;
            }
            _vignette.intensity.value = 0;
        }

        
        private void OnDestroy()
        {
            
            playerEventChannel.RemoveListener<SirenCameraEffect>(SirenTransition);
        }
    }
}