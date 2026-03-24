using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _JYG._Script.Enemy.PatrolSystem
{
    public class PatrolSystem : MonoBehaviour
    {
        [SerializeField] private List<EnemyRoute> enemyPatrolRouteList = new List<EnemyRoute>();
        private NavMeshAgent _navMesh;

        private int _minusRatio = 1;                //인덱스가 감소하는 값
        private int _routeIndex = 0;                //다음 에너미 경로 인덱스

        [field: SerializeField] public bool IsReturnRoute { get; private set; } 
        //에너미의 Index가 0으로 바로 초기화 될지, 천천히 1씩 감소할지
        //true 일때 1씩 감소한다.

        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            _navMesh.SetDestination(enemyPatrolRouteList[_routeIndex].flag.GetFlagPosition()); //0번째 루트를 가도록 설정.
        }

        private void Update()
        {
            if(_navMesh.remainingDistance < 0.5f) //목적지에 도착했다면
            {
                SetEnemyRoute(); //새로 다음 경로를 지정한다.
            }
        }

        private void SetEnemyRoute()
        {
            if (enemyPatrolRouteList.Count - 1 < _routeIndex + _minusRatio || 0 > _routeIndex + _minusRatio) //RouteIndex가 최대치를 벗어나거나 음수로 갔다면
            {
                if (IsReturnRoute) _minusRatio *= -1;       //다시 되돌아가도록 설정했다면 Ratio를 * -1 해주어 1씩 떨어지거나 올라가도록 재설정.
                else _routeIndex = 0;                       //경로가 0부터 시작하도록 설정했다면 Index = 0
            }
            _routeIndex += _minusRatio;

            _navMesh.SetDestination(enemyPatrolRouteList[_routeIndex].flag.GetFlagPosition());             //경로 재설정
            _navMesh.isStopped = true;                                                                          //이동 멈춤
            DOVirtual.DelayedCall(enemyPatrolRouteList[_routeIndex].waitTime, HandleSetEnemyNavmesh, false); //waitTime만큼 기다린 후 경로 지정
        }
        
        public void StartPatrol() => _navMesh.isStopped = false;
        public void StopPatrol() => _navMesh.isStopped = true;

        private void HandleSetEnemyNavmesh()
        {
            _navMesh.isStopped = false; //재이동
        }
    }

    [Serializable]
    public class EnemyRoute
    {
        public float waitTime = 2f;
        public PatrolFlag flag;
        public bool followRotationFlag = false;
    }
}