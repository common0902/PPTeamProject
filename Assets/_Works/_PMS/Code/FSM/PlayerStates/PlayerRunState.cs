using Unity.Cinemachine;
using UnityEngine;

public class PlayerRunState : PlayerMoveBaseState
{
    private CinemachineBasicMultiChannelPerlin _noise;
    protected override void Setup()
    {
        base.Setup();
        _noise = Entity.CinemachineCamera
            .GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public override void Enter()
    {
        base.Enter();
        Movement.SetRunMultiplier(true);
        _noise.FrequencyGain = 4f;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        Movement.SetRunMultiplier(false);
        _noise.FrequencyGain = .5f;
    }
}