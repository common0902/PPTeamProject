using _Script.Agent;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : Agent
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    [SerializeField] private CinemachineCamera _cinemachineCamera;



    #region State에서 참조할 입력 상태값들

    public CinemachineCamera CinemachineCamera => _cinemachineCamera;
    public PlayerMovement Movement { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }
    public Transform CameraTransform { get; private set; }
    public bool IsAttackPressed { get; private set; }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        Movement = GetModule<PlayerMovement>();
        CameraTransform = _cinemachineCamera.transform;
    }

    protected override void AfterInitialize()
    {
        base.AfterInitialize();

        // 입력 이벤트 연결
        PlayerInput.OnMovementChange += OnMovementChange;
        PlayerInput.OnRunStarted += OnRunStarted;
        PlayerInput.OnRunCanceled += OnRunCanceled;
        PlayerInput.OnAttackKeyPressed += OnAttackPressed;

        var stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine?.Setup(this);
    }

    protected override void Update()
    {
        base.Update();
        UpdateRotation();

    }

    private void UpdateRotation()
    {
        Vector3 cameraForward = CameraTransform.forward;
        cameraForward.y = 0;
        if (cameraForward.sqrMagnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.LookRotation(cameraForward);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerInput.OnMovementChange -= OnMovementChange;
        PlayerInput.OnRunStarted -= OnRunStarted;
        PlayerInput.OnRunCanceled -= OnRunCanceled;
        PlayerInput.OnAttackKeyPressed -= OnAttackPressed;
    }

    private void OnMovementChange(Vector2 input)
    {
        MoveInput = input;
    }
    private void OnRunStarted()
    {
        IsRunning = true;
        Debug.Log("OnRunStarted 호출됨");
    }
    private void OnRunCanceled()
    {
        IsRunning = false;
        Debug.Log("OnRunCanceled 호출됨");
    }
    private void OnAttackPressed() => IsAttackPressed = true;

    protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
    {
        if (currentHealth <= 0)
            IsDead = true;
    }


}