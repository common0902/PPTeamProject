using System.Collections;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace _Works._CJW.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [Header("Event Channel")]
        [SerializeField] private EventChannelSO sabotageEvent;
        [Header("Camera Setting")]
        [SerializeField] private float defaultHeight;
        [SerializeField] private float resultHeight;
        [SerializeField] private float durationTime;
        [SerializeField] private AnimationCurve transitionCurve;
        [Header("Quad View Setting")]
        [SerializeField] private float quadViewOffset;
        [SerializeField]private float quadViewDuration;

        private Transform _rootTrs;
        private Transform _playerTrs;
        private Transform _tempTrs;
        private CinemachineThirdPersonFollow _thirdPersonFollow;
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
            _thirdPersonFollow = _camera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineThirdPersonFollow;
        
            Debug.Assert(_thirdPersonFollow != null, "CinemachineThirdPersonFollow component not found on the camera.");
        
            _thirdPersonFollow.VerticalArmLength = defaultHeight;
            _rootTrs = GameObject.Find("CameraRoot").transform;
            _playerTrs = GameObject.Find("Player").transform;
            _tempTrs = new GameObject("CameraTempTransform").transform;
            Debug.Assert(_rootTrs != null, "Player transform not found. Make sure there is a GameObject named 'CameraRoot' in the scene.");
            Debug.Assert(_playerTrs != null, "Player transform not found. Make sure there is a GameObject named 'Player' in the scenes.");
        }
        [ContextMenu("Quad")]
        private void Test()
        {
            TransCameraToQuadView();
        }
        [ContextMenu("First")]
        private void Test1()
        {
            StartCoroutine(TransCameraToFirstViewCoroutine());
        }

        private void TransCameraToQuadView()
        {
            _tempTrs.position = _rootTrs.position;
            _tempTrs.rotation = _rootTrs.rotation;
            _camera.Follow = _tempTrs;
            _tempTrs.DOMove(_playerTrs.position + -(_rootTrs.forward * quadViewOffset), quadViewDuration).SetEase(transitionCurve)
                .OnComplete((() => StartCoroutine(TransCameraToQuadViewCoroutine())));
        }
    
        private IEnumerator TransCameraToQuadViewCoroutine() // 카메라를 쿼드뷰로 변환하는 코루틴
        {
            float t = 0;
            float startVal = _thirdPersonFollow.VerticalArmLength; 
            Quaternion endRotation = Quaternion.Euler(0,0,0);
        
            while (t < durationTime)
            {
                t += Time.deltaTime;
                float percent = t / durationTime;
                float curveValue = transitionCurve.Evaluate(percent);
                _thirdPersonFollow.VerticalArmLength = Mathf.SmoothStep(startVal, resultHeight, percent * curveValue);
                if(percent > 0.35f)
                    _tempTrs.rotation = Quaternion.Slerp(_tempTrs.rotation, endRotation, percent * curveValue);
                yield return null;
            }
            sabotageEvent.RaiseEvent(CameraEvent.TopViewEvent.Init(true));
        }
    
        private IEnumerator TransCameraToFirstViewCoroutine() // 카메라를 1인칭으로 바꾸는 코루틴
        {
            sabotageEvent?.RaiseEvent(CameraEvent.TopViewEvent.Init(false));
            float t = 0;
            float startVal = _thirdPersonFollow.VerticalArmLength;
            Quaternion endRotation = _rootTrs.rotation;
            _tempTrs.position = _rootTrs.position;
            while (t < durationTime)
            {
                t += Time.deltaTime;
                float percent = t / durationTime;
                float curveValue = transitionCurve.Evaluate(percent);
            
                _tempTrs.rotation = Quaternion.Slerp(_tempTrs.rotation, endRotation, percent * curveValue);
                _thirdPersonFollow.VerticalArmLength = Mathf.SmoothStep(startVal, defaultHeight, percent * curveValue);
                yield return null;
            }
            _camera.Follow = _rootTrs;
        }
    }
}
