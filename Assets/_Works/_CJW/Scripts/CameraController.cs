using System;
using System.Collections;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        [Header("Top View Setting")]
        [SerializeField] private Vector3 topViewEuler;
        [SerializeField] private float topViewOffset;
        [SerializeField] private float topViewDuration;
        
        [SerializeField] private float firstViewDelayTime = 0.3f;

        #region Player가 사용하는 변수
        public bool IsTransitioning => _isTransitioning;
        public bool IsTopView => _isTopView;

        private bool _isTransitioning = false;
        private bool _isTopView;

        public event Action OnFirstViewComplete;
        public event Action OnTopViewComplete;
        private bool _hasTopView = false;

        [SerializeField] private CinemachineInputAxisController _inputAxisController;
        #endregion

        private Quaternion _topViewRotation;
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
            _topViewRotation = Quaternion.Euler(topViewEuler);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //탑뷰로 전환
        public void TransToTopView()
        {
            if (!_isTransitioning && !_isTopView)
            {
                _isTransitioning = true;
                _isTopView = true;
                TransCameraToTopView();
            }
        }
        
        //1인칭으로 전환
        public void TransToFirstView()
        {
            if (!_isTransitioning && _isTopView)
            {
                _isTopView = false;
                _isTransitioning = true;
                StartCoroutine(TransCameraToFirstViewCoroutine());
            }
        }
        

        private void TransCameraToTopView()
        {
            _tempTrs.position = _rootTrs.position;
            _tempTrs.rotation = _rootTrs.rotation;
            topViewCam.Follow = _tempTrs;
            _tempTrs.DOMove(_rootTrs.position + -(_rootTrs.forward * topViewOffset), 0.1f).SetEase(transitionCurve)
                .OnComplete((() => StartCoroutine(TransCameraToTopViewCoroutine())));
        }
    
        private IEnumerator TransCameraToTopViewCoroutine()
        {
            _inputAxisController.enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            topViewCam.Priority.Value = 1;
            firstViewCam.Priority.Value = 0;

            float t = 0;

            float startVal = _thirdPersonFollow.VerticalArmLength;

            Quaternion startRotation = _tempTrs.rotation;

            while (t < durationTime)
            {
                t += Time.deltaTime;

                float percent = Mathf.Clamp01(t / durationTime);

                cameraEvent.RaiseEvent(
                    CameraEvent.CameraElapseEvent.Init(percent));

                float curveValue =
                    transitionCurve.Evaluate(percent);

                // 회전
                _tempTrs.rotation =
                    Quaternion.Slerp(
                        startRotation,
                        _topViewRotation,
                        curveValue);

                // 높이
                if (percent < rotateStartPercent)
                {
                    _thirdPersonFollow.VerticalArmLength = startVal;
                }
                else
                {
                    float movePercent = Mathf.InverseLerp
                    (rotateStartPercent, 1f, percent);

                    float heightCurve =
                        transitionCurve.Evaluate(movePercent);

                    _thirdPersonFollow.VerticalArmLength = Mathf.Lerp
                    (startVal, resultHeight, heightCurve);
                }

                yield return null;
            }

            _tempTrs.rotation = _topViewRotation;

            _thirdPersonFollow.VerticalArmLength = resultHeight;

            cameraEvent.RaiseEvent(
                CameraEvent.TopViewEvent.Init(true));

            _isTransitioning = false;

            _hasTopView = true;
            OnTopViewComplete?.Invoke();
        }
        private IEnumerator TransCameraToFirstViewCoroutine()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            cameraEvent?.RaiseEvent(CameraEvent.TopViewEvent.Init(false));

            float t = 0;

            float startVal = _thirdPersonFollow.VerticalArmLength;

            Quaternion startRotation = _tempTrs.rotation;
            Quaternion endRotation = _rootTrs.parent.rotation;

            _tempTrs.position = _rootTrs.position;

            while (t < durationTime)
            {
                t += Time.deltaTime;

                float percent = Mathf.Clamp01(t / durationTime);

                float curveValue = transitionCurve.Evaluate(percent);

                _tempTrs.rotation =
                    Quaternion.Slerp(
                        startRotation,
                        endRotation,
                        curveValue);

                _thirdPersonFollow.VerticalArmLength =
                    Mathf.Lerp(
                        startVal,
                        defaultHeight,
                        curveValue);

                yield return null;
            }

            topViewCam.Follow = _rootTrs;

            topViewCam.Priority.Value = 0;
            firstViewCam.Priority.Value = 1;

            yield return new WaitForSeconds(firstViewDelayTime);

            _isTransitioning = false;

            _inputAxisController.enabled = true;

            if (_hasTopView)
            {
                _hasTopView = false;

                cameraEvent?.RaiseEvent(
                    CameraEvent.FirstViewComplete.Init(true));

                OnFirstViewComplete?.Invoke();
            }
        }
    }
}
