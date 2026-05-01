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
        public EnemyChaseState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _player = GameManager.Instance.Player;
            Debug.Assert(_player != null, $"Player를 찾지 못했음. {agent.gameObject.name}");

            _targetCaster = agent.GetModule<TargetRaycaster>();
            Debug.Assert(_targetCaster != null, $"{agent.gameObject.name}에 TargetRaycaster 모듈 누락");

            _navmesh = agent.GetModule<IAISystem>();
            Debug.Assert(_navmesh != null, $"{agent.gameObject.name}에 IAISystem 모듈 누락");
        }
        public override void Enter()
        {
            _navmesh.Navmesh.isStopped = false;
            _navmesh.Navmesh.speed = _enemy.ChaseSpeed;
            
            _enemy.CallingPartner();    // n초 이내에 사살하지 못하면 모든 Enemy가 플레이어를 쫓는다.
            
            Debug.Log("CHASE!");
            //Test
            if(_targetCaster.TryGetTarget(out GameObject target)) //타겟을 한번 캐스팅 해보고 안닿으면 그때 애니메이션 재생.
            //이렇게 하지 않으면 부자연스럽게 애니메이션이 끊기게 된다.
            {
                if(target.TryGetComponent<TestPPPP>(out TestPPPP test))
                {
                    Debug.Log("플레이어 찾음!!");
                    _enemy.ChangeState((int)EnemyState.ATTACK);
                    return;
                }
            }
            base.Enter(); //애니메이션 재생
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