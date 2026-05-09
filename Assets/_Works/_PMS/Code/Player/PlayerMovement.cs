using _Script.Agent.Modules;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IModule
{
    [SerializeField] private float moveSpeed = 8f, gravity = -9.8f;

    [SerializeField] private float runSpeedMultiplier = 1.5f; // 달리기 배수



    private Vector3 _velocity;
    private float _verticalVelocity;
    private Vector3 _movementDirection;
    private float _currentSpeedMultiplier = 1f; // 현재 스피드 배수
    private CharacterController _characterController;
    private ModuleOwner _owner;

    public bool CanManualMove { get; set; } = true;
    public Vector3 Velocity => _velocity;
    public bool IsGround => _characterController.isGrounded;

    public void Initialize(ModuleOwner owner)
    {
        _owner = owner;
        _characterController = owner.GetComponent<CharacterController>();
    }

    public void SetMovementDirection(Vector3 direction) => _movementDirection = direction;
    public void SetRunMultiplier(bool isRun) 
    {
        _currentSpeedMultiplier = isRun? runSpeedMultiplier : 1f;
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        ApplyGravity();
        Move();
    }


    private void CalculateMovement()
    {
        if (CanManualMove)
            _velocity = _movementDirection;
        else
            _velocity = Vector3.zero;

        _velocity *= moveSpeed * _currentSpeedMultiplier * Time.fixedDeltaTime;

    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
            _verticalVelocity = -0.03f;
        else
            _verticalVelocity += gravity * Time.fixedDeltaTime;

        _velocity.y = _verticalVelocity;
    }

    private void Move()
    {
        _characterController.Move(_velocity);
    }
}