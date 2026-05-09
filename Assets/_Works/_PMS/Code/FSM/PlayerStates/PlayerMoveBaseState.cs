using UnityEngine;
using UnityEngine.Windows;

public abstract class PlayerMoveBaseState : State<PlayerController>
{
    protected PlayerMovement Movement;
    protected Transform CameraTransform;

    protected override void Setup()
    {
        Movement = Entity.GetModule<PlayerMovement>();
        CameraTransform = Entity.CameraTransform;
    }
    public override void Enter()
    {
        UpdateDirection();
    }
    public override void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        Vector3 direction = new Vector3(Entity.MoveInput.x, 0, Entity.MoveInput.y).normalized;
        direction = Quaternion.Euler(0, CameraTransform.eulerAngles.y, 0) * direction;
        Movement.SetMovementDirection(direction);
    }
}