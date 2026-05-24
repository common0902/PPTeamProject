using _Works._CJW.Scripts;
using UnityEngine;

public class PlayerViewMapState : State<PlayerController>
{
    public override void Enter()
    {
        Entity.Visual.SetActive(true);
        Entity.WeaponModule.ResetWeaponState();
        Entity.CamController.TransToTopView();
    }

    public override void Exit()
    {
        Entity.CamController.TransToFirstView();
    }
}