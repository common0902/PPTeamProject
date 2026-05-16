using System;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    [RequireComponent(typeof(BoxCollider))]
    public class Gas : AbstractObject
    {
        private BoxCollider _collider;
        private ParticleSystem _particle;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider>() ;
            _particle = GetComponentInChildren<ParticleSystem>();
            _particle.shape.scale.Scale(_collider.size * 3);;
        }

        protected override void OnTriggerEnterEnemy(AbstractEnemy enemy)
        {
            enemy.ChangeWaterState(true);
        }

        protected override void OnTriggerExitEnemy(AbstractEnemy enemy)
        {
            
            enemy.ChangeWaterState(false);
        }
    }
}