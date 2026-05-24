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
        
        private SabotageVisualModule _visualModule;
        private Rigidbody _rigid;

        public override void Initialize(ModuleOwner moduleOwner)
        {
            base.Initialize(moduleOwner);
        }

        public override void UseFunction()
        {
            base.UseFunction();

            if (GetGround(out var hit))
            {
                _owner.ActiveVisual(false);
                visualObject.SetActive(true);

                float targetY = hit.point.y + (visualObject.transform.lossyScale.y * 0.5f);

                transform.DOMoveY(targetY, 0.4f).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    PlayParticle();

                    DOVirtual.DelayedCall(lifetime, () =>
                    {
                        visualObject.SetActive(false);
                        _owner.ActiveVisual(true);
                        ExecuteDamage();
                    });
                });
            }
        }

        private void ExecuteDamage()
        {
            Collider[] hits = new Collider[maxDetectCount];
            Physics.OverlapBoxNonAlloc(transform.position + boxOffset, boxSize * 0.5F, hits, Quaternion.identity, enemyLayer);
            foreach (Collider c in hits)
            {
                if (c == null) continue;
                
                if (c.TryGetComponent<IDamageable>(out var damageable))
                {
                    Vector3 dir = c.transform.position - transform.position;
                    damageable.TakeDamage(damage, dir.normalized, transform.position);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position + boxOffset, boxSize* 0.5F);
        }
    }
}