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

        private float _lastEnterTime;           //시작한 시간
        private float _searchTime;              //로블록스에서 머리 흔들면서 찾는거마냥 플레이어를 찾는 시간임.

        private EnemyRoute _currentRoute;
        private TargetRaycaster _targetRaycaster;
        public EnemyIdleState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _patrolSystem = agent.GetModule<IAISystem>();
            // enemy = agent as Enemy;

            _targetRaycaster = agent.GetModule<TargetRaycaster>();
        }
        public override void Enter()
        {
            base.Enter();
            _enemy.cautionRatio = -1;
            _currentRoute = _patrolSystem.GetCurrentRoute();
            _lastEnterTime = Time.time;
            _searchTime = _currentRoute.waitTime;
        }

        public override void Update()
        {
            #region Enemy To Chase
            //여기서 만약 타겟을 시야각으로 발견했다면 chase state로 바뀐다.
            //자세한 내용은 AgentState의 Update문 참고.
            if((_viewCaster.IsTargetAttached && _targetRaycaster.TryGetTarget()) || _enemy.GetEnemyCaution > 0)
                _enemy.EnemyFindPlayer();
            
            if (_viewCaster.IsTargetAttached && _targetRaycaster.TryGetTarget()) //Enemy의 시야에 발각되었다! , Enemy의 타겟이 존재한다!
            {
                //여기서 Enemy와 Player와의 Distance를 구하고, 이를 Ratio에 적용한다.
                _enemy.cautionRatio = _viewCaster.Distance / _targetRaycaster.GetDistance2Target(); //멀수록 낮고 가까울수록 증가
                
                if (Mathf.Approximately(_enemy.GetEnemyCaution, 1)) //만약 Enemy의 경계수치가 최고조에 달했다면,
                {
                    _enemy.ChangeState((int)EnemyState.CHASE); //Chase State로 옮겨간다.
                }

                Vector3 lookDir = _targetRaycaster.TargetPlayer.transform.position - _agent.transform.position;
                lookDir.y = 0;
            
                _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation
                    , Quaternion.LookRotation(lookDir) 
                    , Time.deltaTime * 5f);
                //에너미를 플레이어 방향으로 회전시킨다. 이거 자주 쓰여서 따로 함수로 따로 빼야할듯.
                return; //모든 에너미의 행동을 멈추어야 한다.
            }
            else if (_enemy.GetEnemyCaution > 0)
            {
                _enemy.cautionRatio = -1;
                return; //경계치가 다 떨어질때까지 해당 방향 응시
            }
            
            #endregion
            
            if (_lastEnterTime + _searchTime < Time.time)           //잠시 자리에 멈추고, 시간만큼 기다린다.
            {
                _enemy.ChangeState((int)EnemyState.PATROL);
                return;
            }

            if (_patrolSystem.IsArrived())
            {
                _agent.transform.rotation =
                    Quaternion.Slerp(_agent.transform.rotation
                    , _patrolSystem.PrevEnemyRoute.flag.GetFlagTransform().rotation
                    , Time.deltaTime * 2f);
            }
        }
    }
}