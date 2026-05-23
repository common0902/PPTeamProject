using _Script.Agent;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts;
using _Works._PMS.Code.Event;
using System;
using System.Collections;
using TreeEditor;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Agent
{
    [field: SerializeField] public EventChannelSO PlayerEventChannel { get; private set; }

    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [SerializeField] private float knockbackForce = 3f;

    [SerializeField] private float runCooldown = 2f;
    private float _runCooldownTimer;

    [SerializeField] private float viewMapCooldown = 5f;
    private float _viewMapCooldownTimer;


    #region State에서 참조할 입력 상태값들
    public bool IsViewMapCooldown { get; private set; }

    public CinemachineCamera CinemachineCamera => cinemachineCamera;
    public PlayerMovement Movement { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public WeaponModule WeaponModule { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsRunCooldown { get; private set; }
    public Transform CameraTransform { get; private set; }
    public bool IsAttackPressed { get; private set; }
    public bool IsViewMap { get; private set; }
    public CameraController CamController { get; private set; }
    public CinemachineBasicMultiChannelPerlin perlin { get; private set; }

    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        Movement = GetModule<PlayerMovement>();
        WeaponModule = GetModule<WeaponModule>();
        CamController = GetModule<CameraController>();
        perlin = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        CameraTransform = cinemachineCamera.transform;
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

        //PlayerInput.OnWeaponSwapUp += OnWeaponSwapUp;
        //PlayerInput.OnWeaponSwapDown += OnWeaponSwapDown;
        PlayerInput.OnWeaponSwapIndex += OnWeaponSwapIndex;

        CamController.OnFirstViewComplete += OnFirstViewComplete;

        var stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine?.Setup(this);

    }

    

    protected override void Update()
    {
        UpdateRotation();
        UpdateRunCooldown();
        UpdateViewMapCooldown();
        IsAttackPressed = false;
        
    }   

    
    private void UpdateRotation()
    {
        if (CamController.IsTransitioning || CamController.IsTopView) return;

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

    private void UpdateViewMapCooldown()
    {
        if (!IsViewMapCooldown) return;
        _viewMapCooldownTimer -= Time.deltaTime;
        if (_viewMapCooldownTimer <= 0f)
            IsViewMapCooldown = false;
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

        //PlayerInput.OnWeaponSwapUp -= OnWeaponSwapUp;
        //PlayerInput.OnWeaponSwapDown -= OnWeaponSwapDown;
        PlayerInput.OnWeaponSwapIndex -= OnWeaponSwapIndex;

        CamController.OnFirstViewComplete -= OnFirstViewComplete;
    }
    private void OnFirstViewComplete()
    {
        IsViewMapCooldown = true;
        _viewMapCooldownTimer = viewMapCooldown;
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
        PlayerEventChannel.RaiseEvent(PlayerEvents.SprintEndEvent);
    }
    private void OnAttackPressed()
    {
        IsAttackPressed = true;
    }

    private void OnViewMapStarted()
    {
        if (IsViewMapCooldown) return;
        IsViewMap = true;
    }
    
    private void OnViewMapCanceled()
    {
        if (!IsViewMap) return;
        IsViewMap = false;
    }

    private void OnWeaponSwapUp() => WeaponModule?.SwapNext();
    private void OnWeaponSwapDown() => WeaponModule?.SwapPrev();

    private void OnWeaponSwapIndex(int index) => WeaponModule?.SwapWeaponIndex(index);

    protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
    {
        if (currentHealth <= 0)
            IsDead = true;

        if (currentHealth != prevHealth) 
            PlayerEventChannel.RaiseEvent(PlayerEvents.HitEvent.Init(currentHealth));
    }

    

    public override void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition)
    {
        base.TakeDamage(damage, hitDirection, attackerPosition);
        if (hitDirection == Vector3.zero) return;
        Movement.CharacterController.Move(hitDirection.normalized * knockbackForce);
        StartCoroutine(ShakeView());
    }

    private IEnumerator ShakeView()
    {
        float originFrequencyGain = perlin.FrequencyGain;
        float originAmplitudeGain = perlin.AmplitudeGain;
        perlin.FrequencyGain = 5f;
        perlin.AmplitudeGain = 5f;
        yield return new WaitForSeconds(.06f);
        perlin.FrequencyGain = originFrequencyGain;
        perlin.AmplitudeGain = originAmplitudeGain;
    }
}