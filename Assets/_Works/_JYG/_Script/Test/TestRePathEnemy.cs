using System;
using System.Collections;
using _Works._JYG._Script.Enemy;
using _Works._JYG._Script.Enemy.PatrolSystem;
using Agents.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._JYG._Script.Test
{
    public class TestRePathEnemy : MonoBehaviour
    {
        [field: SerializeField] public AbstractEnemy TargetEnemy { get; private set; }

        private void Update()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                StartCoroutine(SetEnemyPos());

            }
        }

        private IEnumerator SetEnemyPos()
        {
            
            IAISystem aiSystem = TargetEnemy.AiSystem;
            aiSystem.RouteRePath(transform.position);
            yield return new WaitForSeconds(0.5f);  //0.5초 딜레이 안걸어주면 Path 재설정 하기 전에 Patrol에서 도착했다고 판단 해 안되네.
            TargetEnemy.ChangeState((int)EnemyState.PATROL);
        }
    }
}
