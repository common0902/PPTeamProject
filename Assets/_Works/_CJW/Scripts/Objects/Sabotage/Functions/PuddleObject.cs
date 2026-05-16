using System;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class PuddleObject : AbstractObject
    {
        [SerializeField] private float detectRadius;

        protected override void OnTriggerEnterEnemy(AbstractEnemy enemy)
        {
        }

        protected override void OnTriggerExitEnemy(AbstractEnemy enemy)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AbstractEnemy enemy))
            {
                // enemy
            }
        }
    }
}