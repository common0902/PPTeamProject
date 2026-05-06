using System.Xml;
using UnityEngine;

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
        AddState<PlayerHitState>(1);
        AddState<PlayerDeadState>(1);
        
    }

    protected override void MakeTransitions()
    {
        // 레이어 0 - 이동
        MakeTransition<PlayerIdleState, PlayerWalkState>(state => Owner.MoveInput.magnitude > 0.1f);

        MakeTransition<PlayerWalkState, PlayerIdleState>(state => Owner.MoveInput.magnitude <= 0.1f);

        MakeTransition<PlayerWalkState, PlayerRunState>(state => Owner.IsRunning);

        MakeTransition<PlayerRunState, PlayerWalkState>(state => !Owner.IsRunning);

        MakeTransition<PlayerRunState, PlayerIdleState>(state => Owner.Movement.Velocity.magnitude <= 0.1f);

        // 레이어 1 - 행동
        MakeAnyTransition<PlayerDeadState>(state => Owner.IsDead, layer: 1);

        MakeAnyTransition<PlayerHitState>(state => !Owner.IsDead && Owner.IsHit, layer: 1);

        MakeTransition<PlayerHitState, PlayerNoneState>(state => Owner.IsHitFinished, layer: 1);
    }
}

    
