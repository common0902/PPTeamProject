using _Script.Agent;
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Agent
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [SerializeField] private float runCooldown = 2f;
    private float _runCooldownTimer;

    #region State에서 참조할 입력 상태값들

    public CinemachineCamera CinemachineCamera => _cinemachineCamera;
    public PlayerMovement Movement { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsRunCooldown { get; private set; }
    public Transform CameraTransform { get; private set; }
    public bool IsAttackPressed { get; private set; }
    public bool IsViewMap { get; private set; }
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
        PlayerInput.OnViewMapStarted += OnViewMapStarted;
        PlayerInput.OnViewMapCanceled += OnViewMapCanceled;


        var stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine?.Setup(this);

    }

    
    protected override void Update()
    {
        UpdateRotation();
        UpdateRunCooldown();
    }   

    
    private void UpdateRotation()
    {
        Vector3 cameraForward = CameraTransform.forward;
        cameraForward.y = 0;
        if (cameraForward.sqrMagnitude > Mathf.Epsilon)
            transform.rotation = Quaternion.LookRotation(cameraForward);
    }
    private void UpdateRunCooldown()
    {
        if (!IsRunCooldown) return;

        _runCooldownTimer -= Time.deltaTime;
        if (_runCooldownTimer <= 0f)
        {
            IsRunCooldown = false;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerInput.OnMovementChange -= OnMovementChange;
        PlayerInput.OnRunStarted -= OnRunStarted;
        PlayerInput.OnRunCanceled -= OnRunCanceled;
        PlayerInput.OnAttackKeyPressed -= OnAttackPressed;
        PlayerInput.OnViewMapStarted -= OnViewMapStarted;
        PlayerInput.OnViewMapCanceled -= OnViewMapCanceled;

    }

    private void OnMovementChange(Vector2 input)
    {
        MoveInput = input;
    }
    private void OnRunStarted()
    {
        if (IsRunCooldown) return; // 쿨타임 중이면 무시
        IsRunning = true;
    }
    private void OnRunCanceled()
    {
        if (!IsRunning) return; // 달리지 않았으면 쿨타임 없음
        IsRunning = false;
        IsRunCooldown = true;
        _runCooldownTimer = runCooldown;
    }
    private void OnAttackPressed() => IsAttackPressed = true;

    private void OnViewMapStarted()
    {
        IsViewMap = true;
    }
    
    private void OnViewMapCanceled()
    {
        IsViewMap = false;
    }

    protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
    {
        if (currentHealth <= 0)
            IsDead = true;
    }


}