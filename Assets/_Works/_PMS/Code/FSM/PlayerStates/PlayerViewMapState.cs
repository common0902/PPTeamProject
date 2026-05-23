using _Works._CJW.Scripts;
using UnityEngine;

public class PlayerViewMapState : State<PlayerController>
{

    public override void Enter()
    {
        Debug.Log("ViewMap 시작");
        Entity.CamController.TransToTopView();
        
    }

    public override void Exit()
    {
        Debug.Log("ViewMap 끝");
        Entity.CamController.TransToFirstView();
    }
}
