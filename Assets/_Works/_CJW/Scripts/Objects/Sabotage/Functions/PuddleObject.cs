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
        private BoxCollider _collider;
        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider>();
        }

        protected override void OnTriggerEnterEnemy(AbstractEnemy enemy)
        {
            enemy.ChangeWaterState(true);
        }
        public override void InitSize(Vector3 size)
        {
            base.InitSize(size);
            _collider.size = size;
        }
        protected override void OnTriggerExitEnemy(AbstractEnemy enemy)
        {
            enemy.ChangeWaterState(false);
        }
        
    }
}