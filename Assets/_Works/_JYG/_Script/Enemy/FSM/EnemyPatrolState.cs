using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Works._JYG._Script.Enemy.PatrolSystem;
using Agents.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyPatrolState : AgentState
    {
        private IAISystem _patrolSystem;
        public EnemyPatrolState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _patrolSystem = agent.GetModule<IAISystem>();
        }

        public override void Enter()
        {
            base.Enter();
            _patrolSystem.Navmesh.isStopped = false;
            Debug.Log("PATROL!");
        }

        public override void Update()
        {
            base.Update();
            //만약 시야각에 적이 있다면 chase로 돌입.
            //2026.04.16 기획 변경됨. IDLE로 유저에게 약간의 시간을 주고, 
            
            //목적지와의 거리가 0.5f 차이라면 경로 재설정, Idle에서 대기
            //만약 ViewCaster (시야각)에 타겟이 찍혔다면, IDLE에서 처리.
            if(_patrolSystem.Navmesh.remainingDistance < 0.5f || _viewCaster.IsTargetAttached)
            {
                _patrolSystem.SetEnemyRoute();
                _enemy.ChangeState((int)EnemyState.IDLE);
            }
            
            if (Keyboard.current.qKey.wasPressedThisFrame) //Debug Only
            {
                _enemy.ChangeState((int)EnemyState.CHASE);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _patrolSystem.Navmesh.isStopped = true;
        }
    }
}