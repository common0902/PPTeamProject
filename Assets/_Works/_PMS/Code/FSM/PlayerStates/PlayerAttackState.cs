
using UnityEngine;

public class PlayerAttackState : State<PlayerController>
{

    public override void Enter()
    {
        Entity.WeaponModule.Attack();
        Debug.Log(11111111);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log(22222222);
    }
}