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
        private ParticleSystem _particle;
        private BoxCollider _collider;
        
        protected override void Awake()
        {
            base.Awake();
            _particle = GetComponentInChildren<ParticleSystem>();
            _collider = GetComponent<BoxCollider>();
        }

        protected override void OnTriggerEnterEnemy(AbstractEnemy enemy)
        {
            enemy.ChangeWaterState(true);
        }
        protected override void HandleTopViewEnd()
        {
            StartCoroutine(FadeOutGas());
        }
        private IEnumerator FadeOutGas()
        {
            yield return new WaitForSeconds(lifeTime);

            _particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ParticleSystem.Particle[] particles =
                new ParticleSystem.Particle[_particle.particleCount];

            int count = _particle.GetParticles(particles);

            
            //현재 파티클들에 남은 lifetime을 2초 또는 2초보다 작으면 현재 시간으로 설정 -> 2초 뒤에 사라짐
            for (int i = 0; i < count; i++)
            {
                particles[i].remainingLifetime =
                    Mathf.Min(particles[i].remainingLifetime, 0);
            }
            _particle.SetParticles(particles, count);
            Destroy(gameObject);
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