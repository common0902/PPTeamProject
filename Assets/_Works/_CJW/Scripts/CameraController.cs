using System;
using System.Collections;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [Header("Event Channel")]
        [SerializeField] private EventChannelSO cameraEvent;
        [Header("Camera Setting")]
        [SerializeField] private float defaultHeight;
        [SerializeField] private float resultHeight;
        [SerializeField] private float durationTime;
        [SerializeField] private AnimationCurve transitionCurve;
        [SerializeField] private CinemachineCamera topViewCam;
        [SerializeField] private CinemachineCamera firstViewCam;
        [Header("Quad View Setting")]
        [SerializeField] private float quadViewOffset;
        [SerializeField]private float quadViewDuration;

        private Transform _rootTrs;
        private Transform _tempTrs;
        private CinemachineThirdPersonFollow _thirdPersonFollow;
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
            _thirdPersonFollow = _camera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineThirdPersonFollow;
        
            Debug.Assert(_thirdPersonFollow != null, "CinemachineThirdPersonFollow component not found on the camera.");
            
            _rootTrs = _camera.Follow;
            // _playerTrs = _rootTrs;
            _tempTrs = new GameObject("CamTempTransform").transform;
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

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Test();
            }
            if(Keyboard.current.tKey.wasPressedThisFrame)
                Test1();
        }

        private void TransCameraToQuadView()
        {
            _tempTrs.position = _rootTrs.position;
            _tempTrs.rotation = _rootTrs.rotation;
            _camera.Follow = _tempTrs;
            _tempTrs.DOMove(_rootTrs.position + -(_rootTrs.forward * quadViewOffset), quadViewDuration).SetEase(transitionCurve)
                .OnComplete((() => StartCoroutine(TransCameraToQuadViewCoroutine())));
        }
    
        private IEnumerator TransCameraToQuadViewCoroutine() // 카메라를 쿼드뷰로 변환하는 코루틴
        {
            topViewCam.Priority.Value = 1;
            firstViewCam.Priority.Value = 0;
            
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
            cameraEvent.RaiseEvent(CameraEvent.TopViewEvent.Init(true));
        }
    
        private IEnumerator TransCameraToFirstViewCoroutine() // 카메라를 1인칭으로 바꾸는 코루틴
        {
            cameraEvent?.RaiseEvent(CameraEvent.TopViewEvent.Init(false));
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
            topViewCam.Priority.Value = 0;
            firstViewCam.Priority.Value = 1;
        }
    }
}
