using System;
using System.Xml.Schema;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy.CombatSystem;
using DG.Tweening;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class FallingSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private Vector3 boxOffset;
        [SerializeField] private int maxDetectCount;
        [SerializeField] private float damage;
        [SerializeField] private float lifetime;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private GameObject visualObject;
        private SabotageVisual _visual;
        private Rigidbody _rigid;

        public override void Initialize(ModuleOwner moduleOwner)
        {
            base.Initialize(moduleOwner);
        }
        public override void UseFunction()
        {
            base.UseFunction();
            visualObject.SetActive(true);
            _owner.ActiveVisual(false, false);
            if (GetGround(out var hit))
            {
                ExecuteDamage();

                //바닥으로 이동하는 코드
                transform.DOMoveY(hit.point.y + 1.5f, 0.25f).SetEase(Ease.InSine).OnComplete((() =>
                {
                    PlayParticle();
                    DOVirtual.DelayedCall(lifetime, () =>
                    {
                        visualObject.SetActive(false);
                        _owner.ActiveVisual(false, false);
                    });
                }));
            }

        }

        private void ExecuteDamage()
        {
            //에너미에 대미지를 가하는 코드
            Collider[] hits = new Collider[maxDetectCount];
            Physics.OverlapBoxNonAlloc(transform.position + boxOffset, boxSize, hits, Quaternion.identity,enemyLayer);
            foreach (Collider c in hits)
            {
                if(c == null) continue;
                if (c.TryGetComponent<IDamageable>(out var damageable))
                {
                    Vector3 dir = c.transform.position - transform.position;
                    damageable.TakeDamage
                        (damage, dir.normalized, transform.position);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position + boxOffset, boxSize);
        }
    }
}