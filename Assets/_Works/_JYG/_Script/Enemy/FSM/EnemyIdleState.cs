using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Works._JYG._Script.Enemy.PatrolSystem;
using Agents.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyIdleState : AgentState
    {
        //Enemy를 받아와서 ChangeState를 해주어야한다.
        private IAISystem _patrolSystem;

        private float _lastEnterTime;
        private float _searchTime;

        private EnemyRoute _currentRoute;
        private AbstractEnemy _enemy;
        public EnemyIdleState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _enemy = agent as AbstractEnemy;
            _patrolSystem = agent.GetModule<IAISystem>();
            // enemy = agent as Enemy;
        }
        public override void Enter()
        {
            base.Enter();
            _currentRoute = _patrolSystem.CurrentRoute();
            _lastEnterTime = Time.time;
            _searchTime = _currentRoute.waitTime;

            Debug.Log("IDLE!");
        }

        public override void Update()
        {
            base.Update();
            if (_lastEnterTime + _searchTime < Time.time)
            {
                _enemy.ChangeState((int)EnemyState.PATROL);
                return;
            }

            if (_patrolSystem.PrevEnemyRoute.setRotationFlag)
            {
                _agent.transform.rotation =
                    Quaternion.Slerp(_agent.transform.rotation
                    , _patrolSystem.PrevEnemyRoute.flag.GetFlagTransform().rotation
                    , Time.deltaTime * 2f);
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                _enemy.ChangeState((int)EnemyState.CHASE);
            }
            //여기서 만약 타겟을 시야각으로 발견했다면 chase state로 바뀐다.
        }
    }
}