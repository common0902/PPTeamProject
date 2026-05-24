using System.Collections;
using System.Collections.Generic;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    [RequireComponent(typeof(BoxCollider))]
    public class Gas : AbstractObject
    {
        private List<AbstractEnemy> _enemyList = new List<AbstractEnemy>();
        
        private BoxCollider _collider;
        private ParticleSystem _particle;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider>();
            _particle = GetComponentInChildren<ParticleSystem>();
            // _particle.shape.scale.Scale(_collider.size * 3);;
        }

        protected override void HandleTopViewEnd()
        {
            StartCoroutine(FadeOutGas());
        }

        public override void InitSize(Vector3 size)
        {
            base.InitSize(size);
            _collider.size = size;
            var shape = _particle.shape;
            shape.scale = size * 3 / 2;
        }

        private IEnumerator FadeOutGas()
        {
            yield return new WaitForSeconds(lifeTime - 2f);

            _particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ParticleSystem.Particle[] particles =
                new ParticleSystem.Particle[_particle.particleCount];

            int count = _particle.GetParticles(particles);

            
            //현재 파티클들에 남은 lifetime을 2초 또는 2초보다 작으면 현재 시간으로 설정 -> 2초 뒤에 사라짐
            for (int i = 0; i < count; i++)
            {
                particles[i].remainingLifetime =
                    Mathf.Min(particles[i].remainingLifetime, 2f);
            }
            // 파티클들을 저장함.
            _particle.SetParticles(particles, count);
            // 2초 뒤에 이 오브젝트를 지움
            Destroy(gameObject, 2f);
        }
        protected override void OnTriggerEnterEnemy(AbstractEnemy enemy)
        {
            _enemyList.Add(enemy);
            enemy.ChangeWaterState(true);
            enemy.EnemyOutline.enabled = true;
            enemy.GetModule<TargetRaycaster>().enabled = false;
        }

        protected override void OnTriggerExitEnemy(AbstractEnemy enemy)
        {
            _enemyList.Remove(enemy);
            enemy.ChangeWaterState(false);
            enemy.EnemyOutline.enabled = false;
            enemy.GetModule<TargetRaycaster>().enabled = true;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (AbstractEnemy enemy in _enemyList)
            {
                if (enemy == null) continue;
                enemy.ChangeWaterState(false);
                enemy.EnemyOutline.enabled = false;
                enemy.GetModule<TargetRaycaster>().enabled = true;
            }
        }
    }
}