using System.Collections.Generic;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class ShortCircuitSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private List<AbstractEnemy> targetEnemies;
        public override void UseFunction()
        {
            foreach (AbstractEnemy enemy in targetEnemies)
            {
                // enemy.
            }
        }
    }
}