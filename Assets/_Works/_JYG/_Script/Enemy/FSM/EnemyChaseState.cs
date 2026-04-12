using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Works._JYG._Script.Enemy.PatrolSystem;
using Agents.FSM;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyChaseState : AgentState
    {
        private IAISystem _navmesh;
        private GameObject _player; //나중에 Player Component로 대체해야한다.
        private TargetRaycaster _targetCaster;
        private AbstractEnemy _enemy;
        public EnemyChaseState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _enemy = agent as AbstractEnemy;

            _player = GameManager.Instance.Player;
            Debug.Assert(_player != null, $"Player를 찾지 못했음. {agent.gameObject.name}");

            _targetCaster = agent.GetModule<TargetRaycaster>();
            Debug.Assert(_targetCaster != null, $"{agent.gameObject.name}에 TargetRaycaster 모듈 누락");

            _navmesh = agent.GetModule<IAISystem>();
            Debug.Assert(_navmesh != null, $"{agent.gameObject.name}에 IAISystem 모듈 누락");
        }
        public override void Enter()
        {
            base.Enter();
            _navmesh.Navmesh.isStopped = false;

            Debug.Log("CHASE!");
        }

        public override void Update()
        {
            _navmesh.Navmesh.SetDestination(_player.transform.position);

            if(_targetCaster.TryGetTarget(out GameObject target))
            {
                if(target.TryGetComponent<TestPPPP>(out TestPPPP test))
                {
                    Debug.Log("플레이어 찾음!!");
                    _enemy.ChangeState((int)EnemyState.ATTACK);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            _navmesh.Navmesh.isStopped = true;
        }
    }
}