using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Works._CJW.Scripts.Rendering
{
    public class EmergencyImage : MonoBehaviour
    {
        [SerializeField] private int transitionCount;
        [SerializeField] private float transitionTime;
        [SerializeField] private Color minValue;
        [SerializeField] private Color maxValue;
        private Image _image;
        private WaitForSeconds _waitT;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _waitT = new WaitForSeconds(transitionTime);
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
            _image.enabled = true;
            yield return _waitT;
        }

        private IEnumerator FadeOut()
        {
            _image.enabled = false;
            yield return _waitT;
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