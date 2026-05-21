using _Works._CJW.Scripts;
using UnityEngine;

public class PlayerViewMapState : State<PlayerController>
{
    private CameraController _cameraController;

    protected override void Setup()
    {
        _cameraController = Entity.GetModule<CameraController>();
    }

    public override void Enter()
    {

        
    }
}
