using System.Collections.Generic;
using _Works._JYG._Script.Enemy;
using Agents.FSM;
using DG.Tweening;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class ShortCircuitSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private List<AbstractEnemy> targetEnemies;
        [SerializeField] private float duration;
        public override void UseFunction()
        {
            PlayParticle();
            foreach (AbstractEnemy enemy in targetEnemies)
            {
                enemy.AiSystem.RouteRePath(transform.position);
                enemy.ChangeState((int)EnemyState.PATROL);
            }
        }
    }
}