using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerIdleState : State<PlayerController>
{
    private PlayerMovement _movement;

    protected override void Setup()
    {
        _movement = Entity.GetModule<PlayerMovement>();
    }

    public override void Enter()
    {
        _movement.SetMovementDirection(Vector2.zero);

    }
}
