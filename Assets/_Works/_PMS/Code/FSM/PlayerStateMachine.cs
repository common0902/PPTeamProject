//using System.Xml;
//using UnityEngine;

//public class PlayerStateMachine : MonoStateMachine<PlayerController>
//{
//    protected override void AddStates()
//    {
//        AddState<EntityDefaultState>();
//        AddState<RollingState>();
//        AddState<DeadState>();
//    }

//    protected override void MakeTransitions()
//    {
//        // Default 상태에서 롤링이 켜지면 롤링으로
//        MakeTransition<EntityDefaultState, RollingState>(state => Owner.Movement?.IsRolling ?? false);

//        // Rolling 상태에서 롤링이 꺼지면 디폴트로
//        MakeTransition<RollingState, EntityDefaultState>(state => !Owner.Movement.IsRolling);

//        // Dead 상태에서 죽음이 해제 되면 디폴트로
//        MakeTransition<DeadState, EntityDefaultState>(state => !Owner.IsDead);

//        //언제든 디폴트 상태로 가라는 명령을 받으면 이동
//        MakeAnyTransition<EntityDefaultState>(EntityStateCommand.ToDefaultState);
//        //언제든 사망하면 Dead로 이동
//        MakeAnyTransition<DeadState>(state => Owner.IsDead);
//    }
//}

//    }
