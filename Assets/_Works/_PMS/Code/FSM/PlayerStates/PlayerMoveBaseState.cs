using UnityEngine;
using UnityEngine.Windows;

public abstract class PlayerMoveBaseState : State<PlayerController>
{
    protected PlayerMovement Movement;
    protected PlayerInputSO Input;

    protected override void Setup()
    {
        Movement = Entity.GetModule<PlayerMovement>();
        Input = Entity.PlayerInput;
    }

    public override void Enter()
    {
        Input.OnMovementChange += OnMovementChange;
        Movement.SetMovementDirection(Entity.MoveInput);
    }

    public override void Exit()
    {
        Input.OnMovementChange -= OnMovementChange;
    }

    private void OnMovementChange(Vector2 dir)
    {
        Movement.SetMovementDirection(dir);
    }
}