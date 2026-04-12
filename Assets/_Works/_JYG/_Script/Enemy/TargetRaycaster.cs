using _Script.Agent;
using _Script.Agent.Modules;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace _Works._JYG._Script.Enemy
{
    public class TargetRaycaster : MonoBehaviour, IModule
    {
        private ModuleOwner _owner;
        private GameObject _player;
        [field: SerializeField] public float Range { get; private set; } = 80f;
        public GameObject Target{ get; private set; }
        [SerializeField] private LayerMask targetMask;
        public void Initialize(ModuleOwner moduleAgent)
        {
            _owner = moduleAgent;
            _player = GameManager.Instance.Player;
        }

        private void FixedUpdate()
        {
            GetTarget(_player.transform);
        }

        private void GetTarget(Transform target)
        {
            Ray ray = new Ray(transform.position, target.transform.position - transform.position); // 내 위치에서 플레이어 위치 방향으로 레이캐스트 발사.
            Debug.DrawRay(ray.origin, ray.direction * Range, Color.cyan);                            // 
            if (Physics.Raycast(ray, out RaycastHit hit, Range, targetMask))                                // 레이캐스트로 Target찾기.
            {
                Target = hit.transform.gameObject;
                return;
            }
            Target = null;
        }

        public bool TryGetTarget(out GameObject target) //TryGetComponent 참고함. Target 있으면 반환
        {
            target = Target;
            return Target != null;
        }
        public bool TryGetTarget()                      //Target이 존재만 한다면 True 반환
        {
            return Target != null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * Range);
        }
    }
}