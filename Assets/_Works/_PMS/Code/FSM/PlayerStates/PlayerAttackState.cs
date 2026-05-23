
using UnityEngine;

public class PlayerAttackState : State<PlayerController>
{

    public override void Enter()
    {
        Entity.WeaponModule.Attack();
    }

}   