using System;
using System.Collections;
using _Works._JYG._Script.Enemy;
using _Works._JYG._Script.Enemy.PatrolSystem;
using Agents.FSM;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class PuddleObject : AbstractObject
    {
        protected override void OnTriggerEnterEnemy(AbstractEnemy enemy)
        {
            enemy.isWater = true;
        }

        protected override void OnTriggerExitEnemy(AbstractEnemy enemy)
        {
            enemy.isWater = false;
        }
        

    }
}