using UnityEngine;

public class PlayerRunState : PlayerMoveBaseState
{
    public override void Enter()
    {
        base.Enter();
        Movement.SetRunMultiplier(true);
    }

    public override void Exit()
    {
        base.Exit();
        Movement.SetRunMultiplier(false);
    }
}