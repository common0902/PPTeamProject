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
            // 적의 이동속도가 내려가고 시아가 차단된다.
        }

        protected override void OnTriggerExitEnemy(AbstractEnemy enemy)
        {
            
        }
    }
}