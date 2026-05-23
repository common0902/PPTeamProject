using System.Xml;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerStateMachine : MonoStateMachine<PlayerController>
{
    // Player의 레이어별 상태 추가
    protected override void AddStates()
    {
        // 레이어 0: 이동 관련한 로직
        AddState<PlayerIdleState>();
        AddState<PlayerWalkState>();
        AddState<PlayerRunState>();

        // 레이어 1: 행동에 대한 로직
        AddState<PlayerNoneState>(1);
        AddState<PlayerAttackState>(1);
        AddState<PlayerDeadState>(1);
        AddState<PlayerViewMapState>(1);
        
    }

    protected override void MakeTransitions()
    {
        // 레이어 0: 이동
        MakeTransition<PlayerIdleState, PlayerWalkState>(state => !Owner.CamController.IsTransitioning && !Owner.CamController.IsTopView && Owner.MoveInput.magnitude > 0.1f);

        MakeTransition<PlayerIdleState, PlayerRunState>(state => !Owner.CamController.IsTransitioning && !Owner.CamController.IsTopView && Owner.MoveInput.magnitude > 0.1f && Owner.IsRunning && !Owner.IsRunCooldown);

        MakeTransition<PlayerWalkState, PlayerIdleState>(state => Owner.CamController.IsTransitioning || Owner.CamController.IsTopView || Owner.MoveInput.magnitude <= 0.1f);

        MakeTransition<PlayerWalkState, PlayerRunState>(state => !Owner.CamController.IsTransitioning && !Owner.CamController.IsTopView && Owner.IsRunning && !Owner.IsRunCooldown);

        MakeTransition<PlayerRunState, PlayerWalkState>(state => Owner.CamController.IsTransitioning || Owner.CamController.IsTopView || !Owner.IsRunning);

        MakeTransition<PlayerRunState, PlayerIdleState>(state => Owner.Movement.Velocity.magnitude <= 0.1f);    

        // 레이어 1: 행동
        MakeAnyTransition<PlayerDeadState>(state => Owner.IsDead, layer: 1);

        MakeAnyTransition<PlayerAttackState>(state => !Owner.IsDead && !Owner.IsViewMap && !Owner.CamController.IsTransitioning && Owner.IsAttackPressed && Owner.WeaponModule.CanAttack, layer: 1);

        MakeAnyTransition<PlayerViewMapState>(state => !Owner.IsDead && !Owner.IsViewMapCooldown && Owner.IsViewMap, layer: 1);

        MakeTransition<PlayerAttackState, PlayerNoneState>(state => true, layer: 1);

        MakeTransition<PlayerViewMapState, PlayerNoneState>(state => !Owner.IsDead && !Owner.IsViewMap, layer: 1);
    }
}

    
