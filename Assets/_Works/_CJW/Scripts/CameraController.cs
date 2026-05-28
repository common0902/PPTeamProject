using System;
using System.Collections;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._JYG._Script;
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

        [Header("Camera References")]
        [SerializeField] private CinemachineCamera topViewCam;
        [SerializeField] private CinemachineCamera firstViewCam;
        [SerializeField] private CinemachineInputAxisController _inputAxisController;

        [Header("Top View Transition")]
        [SerializeField] private Vector3 topViewEuler;
        [SerializeField] private float topViewOffset;
        [Tooltip("탑뷰 전환 총 지속 시간 (초)")]
        [SerializeField] private float topViewDuration = 1.0f;
        [Tooltip("탑뷰 전환 시 높이 이동을 시작하는 퍼센트 지점 (0~1)")]
        [SerializeField] [Range(0f, 1f)] private float rotateStartPercent = 0.4f;
        [Tooltip("탑뷰 카메라 최종 높이")]
        [SerializeField] private float resultHeight;
        [SerializeField] private AnimationCurve topViewCurve;

        [Header("First View Transition")]
        [Tooltip("1인칭 전환 총 지속 시간 (초)")]
        [SerializeField] private float firstViewDuration = 1.0f;
        [Tooltip("1인칭 전환 후 딜레이 시간 (초)")]
        [SerializeField] private float firstViewDelayTime = 0.3f;
        [Tooltip("1인칭 기본 카메라 높이")]
        [SerializeField] private float defaultHeight;
        [SerializeField] private AnimationCurve firstViewCurve;

        #region 상태 프로퍼티
        public bool IsTransitioning => _isTransitioning;
        public bool IsTopView => _isTopView;

        private bool _isTransitioning = false;
        private bool _isTopView = false;
        private bool _hasTopView = false;

        public event Action OnFirstViewComplete;
        public event Action OnTopViewComplete;
        #endregion

        private Quaternion _topViewRotation;
        private Transform _rootTrs;
        private Transform _tempTrs;
        private CinemachineThirdPersonFollow _thirdPersonFollow;

        public void Initialize(ModuleOwner moduleOwner)
        {
            _thirdPersonFollow = topViewCam.GetCinemachineComponent(CinemachineCore.Stage.Body)
                                 as CinemachineThirdPersonFollow;

            Debug.Assert(_thirdPersonFollow != null,
                "CinemachineThirdPersonFollow component not found on the camera.");

            _rootTrs = topViewCam.Follow;
            _tempTrs = new GameObject("CamTempTransform").transform;
            _topViewRotation = Quaternion.Euler(topViewEuler);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        public void TransToTopView()
        {
            if (_isTransitioning || _isTopView) return;

            _isTransitioning = true;
            _isTopView = true;
            BeginTopViewTransition();
        }

        public void TransToFirstView()
        {
            if (_isTransitioning || !_isTopView) return;

            _isTopView = false;
            _isTransitioning = true;
            StartCoroutine(FirstViewTransitionCoroutine());
        }

        private void BeginTopViewTransition()
        {
            _tempTrs.position = _rootTrs.position;
            _tempTrs.rotation = _rootTrs.rotation;
            topViewCam.Follow = _tempTrs;

            // 카메라를 살짝 뒤로 빼준 뒤 코루틴 시작
            _tempTrs
                .DOMove(_rootTrs.position + -(_rootTrs.forward * topViewOffset), 0.1f)
                .OnComplete(() => StartCoroutine(TopViewTransitionCoroutine()));
        }

        private IEnumerator TopViewTransitionCoroutine()
        {
            // 충돌 및 입력 비활성화
            GameManager.Instance.Player.GetComponent<CapsuleCollider>().enabled = false;
            _inputAxisController.enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            topViewCam.Priority.Value = 1;
            firstViewCam.Priority.Value = 0;

            float elapsed = 0f;
            float startHeight = _thirdPersonFollow.VerticalArmLength;
            Quaternion startRotation = _tempTrs.rotation;

            while (elapsed < topViewDuration)
            {
                elapsed += Time.deltaTime;
                float percent = Mathf.Clamp01(elapsed / topViewDuration);
                float curveValue = topViewCurve.Evaluate(percent);

                // 이벤트: 진행률 전달
                cameraEvent.RaiseEvent(CameraEvent.CameraElapseEvent.Init(percent));

                // 회전 보간
                _tempTrs.rotation = Quaternion.Slerp(startRotation, _topViewRotation, curveValue);

                // 높이 보간 (rotateStartPercent 이후에 시작)
                if (percent >= rotateStartPercent)
                {
                    float heightPercent = Mathf.InverseLerp(rotateStartPercent, 1f, percent);
                    float heightCurve   = topViewCurve.Evaluate(heightPercent);
                    _thirdPersonFollow.VerticalArmLength = Mathf.Lerp(startHeight, resultHeight, heightCurve);
                }

                yield return null;
            }

            // 최종값 스냅
            _tempTrs.rotation = _topViewRotation;
            _thirdPersonFollow.VerticalArmLength = resultHeight;

            cameraEvent.RaiseEvent(CameraEvent.TopViewEvent.Init(true));

            _isTransitioning = false;
            _hasTopView = true;
            OnTopViewComplete?.Invoke();
        }

        private IEnumerator FirstViewTransitionCoroutine()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            cameraEvent?.RaiseEvent(CameraEvent.TopViewEvent.Init(false));

            float elapsed = 0f;
            float startHeight   = _thirdPersonFollow.VerticalArmLength;
            Quaternion startRot = _tempTrs.rotation;
            Quaternion endRot   = _rootTrs.parent.rotation;

            _tempTrs.position = _rootTrs.position;

            while (elapsed < firstViewDuration)
            {
                elapsed += Time.deltaTime;
                float percent    = Mathf.Clamp01(elapsed / firstViewDuration);
                float curveValue = firstViewCurve.Evaluate(percent);

                // 회전 및 높이 보간
                _tempTrs.rotation = Quaternion.Slerp(startRot, endRot, curveValue);
                _thirdPersonFollow.VerticalArmLength = Mathf.Lerp(startHeight, defaultHeight, curveValue);

                yield return null;
            }
            // Follow 원래 Transform으로 복구 및 카메라 우선순위 전환
            topViewCam.Follow = _rootTrs;
            topViewCam.Priority.Value  = 0;
            firstViewCam.Priority.Value = 1;

            yield return new WaitForSeconds(firstViewDelayTime);

            _isTransitioning = false;
            _inputAxisController.enabled = true;

            if (_hasTopView)
            {
                _hasTopView = false;
                
                cameraEvent?.RaiseEvent(CameraEvent.FirstViewComplete.Init(true));
                OnFirstViewComplete?.Invoke();
                GameManager.Instance.Player.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }
}