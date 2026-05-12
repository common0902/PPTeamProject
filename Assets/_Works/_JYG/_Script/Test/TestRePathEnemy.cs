using System;
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
                IAISystem aiSystem = TargetEnemy.AiSystem;
                aiSystem.RouteRePath(transform.position);
                TargetEnemy.ChangeState((int)EnemyState.PATROL);

            }
        }
    }
}
