using System.Xml.Schema;
using _Script.Agent.Modules;
using _Works._JYG._Script.Enemy.CombatSystem;
using DG.Tweening;
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
        private Rigidbody _rigid;

        public override void Initialize(ModuleOwner moduleOwner)
        {
            base.Initialize(moduleOwner);
            gameObject.SetActive(false);
        }

        public override void UseFunction()
        {
            gameObject.SetActive(true);
            if (GetGround(out var hit))
            {
                ExecuteDamage();

                //바닥으로 이동하는 코드
                transform.DOMoveY(hit.point.y + 1.5f, 0.5f).OnComplete((() =>
                {
                    DOVirtual.DelayedCall(lifetime, () =>
                    {
                        gameObject.SetActive(false);
                    });
                }));
            }

        }

        private void ExecuteDamage()
        {
            //에너미에 대미지를 가하는 코드
            Collider[] hits = new Collider[maxDetectCount];
            Physics.OverlapBoxNonAlloc(transform.position, boxSize / 2, hits);
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
            Gizmos.color = Color.chartreuse;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, -10, transform.position.z));
            Gizmos.DrawCube(transform.position + boxOffset, boxSize) ;
        }
    }
}