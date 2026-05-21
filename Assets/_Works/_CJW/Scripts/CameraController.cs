using System;
using System.Collections;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts
{
    public class CameraController : MonoBehaviour, IModule
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
        [SerializeField] private float quadViewDuration;

        private bool _isTransitioning = false;
        private Transform _rootTrs;
        private Transform _tempTrs;
        private CinemachineThirdPersonFollow _thirdPersonFollow;
        [SerializeField] private float rotateStartPercent = 0.4f;

        public void Initialize(ModuleOwner moduleOwner)
        {
            _thirdPersonFollow = topViewCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineThirdPersonFollow;
        
            Debug.Assert(_thirdPersonFollow != null, "CinemachineThirdPersonFollow component not found on the camera.");
            
            _rootTrs = topViewCam.Follow;
            // _playerTrs = _rootTrs;
            _tempTrs = new GameObject("CamTempTransform").transform;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Awake()
        {
            _thirdPersonFollow = topViewCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineThirdPersonFollow;
        
            Debug.Assert(_thirdPersonFollow != null, "CinemachineThirdPersonFollow component not found on the camera.");
            
            _rootTrs = topViewCam.Follow;
            // _playerTrs = _rootTrs;
            _tempTrs = new GameObject("CamTempTransform").transform;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //탑뷰로 전환
        public void TransToTopView()
        {
            if (!_isTransitioning)
            {
                _isTransitioning = true;
                TransCameraToQuadView();
            }
        }
        
        //1인칭으로 전환
        public void TransToFirstView()
        {
            if (!_isTransitioning)
            {
                _isTransitioning = true;
                StartCoroutine(TransCameraToFirstViewCoroutine());
            }
        }

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                TransToTopView();
            }
            if(Keyboard.current.tKey.wasPressedThisFrame)
                TransToFirstView();
        }

        private void TransCameraToQuadView()
        {
            _tempTrs.position = _rootTrs.position;
            _tempTrs.rotation = _rootTrs.rotation;
            topViewCam.Follow = _tempTrs;
            _tempTrs.DOMove(_rootTrs.position + -(_rootTrs.forward * quadViewOffset), 0.1f).SetEase(transitionCurve)
                .OnComplete((() => StartCoroutine(TransCameraToQuadViewCoroutine())));
        }
    
        private IEnumerator TransCameraToQuadViewCoroutine() //탑뷰로 올라가기 시작
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
            topViewCam.Priority.Value = 1;
            firstViewCam.Priority.Value = 0;

            float t = 0;

            float startVal = _thirdPersonFollow.VerticalArmLength;


            while (t < durationTime)
            {
                t += Time.deltaTime;

                float percent = Mathf.Clamp01(t / durationTime);
                cameraEvent.RaiseEvent(CameraEvent.CameraElapseEvent.Init(percent));
            
                // 0.4 이전까지는 그대로
                if (percent < rotateStartPercent)
                {
                    _thirdPersonFollow.VerticalArmLength = startVal;
                }
                else
                {
                    // 0.4 ~ 1 구간을 다시 0 ~ 1로 변환
                    float movePercent =
                        Mathf.InverseLerp(rotateStartPercent, 1f, percent);

                    float curveValue =
                        transitionCurve.Evaluate(movePercent);

                    _thirdPersonFollow.VerticalArmLength =
                        Mathf.Lerp(startVal, resultHeight, curveValue);
                }

                yield return null;
            }

            _thirdPersonFollow.VerticalArmLength = resultHeight;

            cameraEvent.RaiseEvent(CameraEvent.TopViewEvent.Init(true));
            _isTransitioning = false;
        }
        private IEnumerator TransCameraToFirstViewCoroutine() // 카메라를 1인칭으로 바꾸는 코루틴
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
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
            topViewCam.Follow = _rootTrs;
            topViewCam.Priority.Value = 0;
            firstViewCam.Priority.Value = 1;
            _isTransitioning = false;
        }
    }
}
