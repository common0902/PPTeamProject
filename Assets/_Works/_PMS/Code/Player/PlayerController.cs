using _Script.Agent;
using UnityEngine;

public class PlayerController : Agent
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

    public PlayerMovement Movement { get; private set; }

    #region State에서 참조할 입력 상태값들
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsAttackPressed { get; private set; }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        Movement = GetModule<PlayerMovement>();
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

    protected override void OnDestroy()
    {
        PlayerInput.OnMovementChange -= OnMovementChange;
        PlayerInput.OnRunStarted -= OnRunStarted;
        PlayerInput.OnRunCanceled -= OnRunCanceled;
        PlayerInput.OnAttackKeyPressed -= OnAttackPressed;
    }

    private void OnMovementChange(Vector2 input)
    {
        MoveInput = input;
    }
    private void OnRunStarted() => IsRunning = true;
    private void OnRunCanceled() => IsRunning = false;
    private void OnAttackPressed() => IsAttackPressed = true;

    protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
    {
        if (currentHealth <= 0)
            IsDead = true;
    }
}