using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float defaultHeight;
    [SerializeField] private float resultHeight;
    [SerializeField] private float durationTime;
    [SerializeField] private AnimationCurve transitionCurve;
    private CinemachineThirdPersonFollow _thirdPersonFollow;
    private CinemachineCamera _camera;
    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
        _thirdPersonFollow = _camera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineThirdPersonFollow;
        
        Debug.Assert(_thirdPersonFollow != null, "CinemachineThirdPersonFollow component not found on the camera.");
        
        _thirdPersonFollow.VerticalArmLength = defaultHeight;
    }
    [ContextMenu("Quad")]
    private void Test()
    {
        StartCoroutine(TransCameraToQuad());
    }
    [ContextMenu("Third")]
    private void Test1()
    {
        StartCoroutine(TransCameraToThird());
    }

    private IEnumerator TransCameraToQuad()
    {
        float t = 0;
        float startVal = _thirdPersonFollow.VerticalArmLength;
        
        while (t < durationTime)
        {
            t += Time.deltaTime;
            float percent = t / durationTime;
            float curveValue = transitionCurve.Evaluate(percent);
            _thirdPersonFollow.VerticalArmLength = Mathf.SmoothStep(startVal, resultHeight, percent * curveValue);
            yield return null;
        }
    }
    
    private IEnumerator TransCameraToThird()
    {
        float t = 0;
        float startVal = _thirdPersonFollow.VerticalArmLength;
        
        while (t < durationTime)
        {
            t += Time.deltaTime;
            float percent = t / durationTime;
            float curveValue = transitionCurve.Evaluate(percent);
            _thirdPersonFollow.VerticalArmLength = Mathf.SmoothStep(startVal, defaultHeight, percent * curveValue);
            yield return null;
        }
    }
}
