using System;
using System.Collections.Generic;
using _Script.Agent;
using _Script.Agent.Modules;
using UnityEngine;
using UnityEngine.AI;

namespace _Works._JYG._Script.Enemy.PatrolSystem
{
    public class EnemyAISystem : MonoBehaviour, IAISystem, IModule
    {
        private Agent _agent;

        [SerializeField] private List<EnemyRoute> enemyPatrolRouteList = new List<EnemyRoute>();
        public NavMeshAgent Navmesh { get; private set; }

        private int _minusRatio = 1;                //인덱스가 감소하는 값
        private int _routeIndex = -1;                //다음 에너미 경로 인덱스

        [field: SerializeField] public bool IsReturnRoute { get; private set; } = true;
        //에너미의 Index가 0으로 바로 초기화 될지, 천천히 1씩 감소할지
        //true 일때 1씩 감소한다.

        public EnemyRoute PrevEnemyRoute { get; private set; }
        private EnemyRoute _currentRoute;

        public void Initialize(ModuleOwner owner)
        {
            _agent = owner as Agent;
            Navmesh = _agent.GetComponent<NavMeshAgent>();
            Debug.Assert(Navmesh != null, $"Owner에 Navmesh가 존재하지 않습니다! {gameObject.name}.");
            
            _currentRoute = enemyPatrolRouteList[0];
            PrevEnemyRoute = _currentRoute;

        }

        public void SetEnemyRoute()
        {
            if (enemyPatrolRouteList.Count - 1 < _routeIndex + _minusRatio || 0 > _routeIndex + _minusRatio) //RouteIndex가 최대치를 벗어나거나 음수로 갔다면
            {
                if (IsReturnRoute) _minusRatio *= -1;       //다시 되돌아가도록 설정했다면 Ratio를 * -1 해주어 1씩 떨어지거나 올라가도록 재설정.
                else _routeIndex = -1;                       //경로가 0부터 시작하도록 설정했다면 Index = 0 (이후에 +1 해주니까 -1이다.)
            }
            _routeIndex += _minusRatio;

            PrevEnemyRoute = _currentRoute;
            EnemyRoute targetRoute = enemyPatrolRouteList[_routeIndex];

            Navmesh.SetDestination(targetRoute.flag.GetFlagPosition());             //경로 재설정

            _currentRoute = targetRoute;
        }

        public void StartMove() => Navmesh.isStopped = false;
        public void StopMove() => Navmesh.isStopped = true;
        public EnemyRoute CurrentRoute() => _currentRoute;
    }

    [Serializable]
    public class EnemyRoute
    {
        public float waitTime = 2f;             //기다리고 시작하는 시간
        public PatrolFlag flag;                 //이동 경로 flag
        public bool setRotationFlag = true;     //flag 방향으로 회전할것인가?
    }
}