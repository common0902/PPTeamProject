using UnityEngine;

namespace _Works._JYG._Script.Test
{
    public class TestView : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 360)]
        float _viewAngle;
        [SerializeField]
        float _range;

        // 타겟의 레이어와 장애물의 레이어
        LayerMask _targetMask;
        LayerMask _obstacleMask;

        private void Awake()
        {
            _viewAngle = 90f;
            _range = 5f;
            _targetMask = LayerMask.GetMask("Enemy");
            _obstacleMask = LayerMask.GetMask("Ignore Raycast");
        }

        // 범위 시각화
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, _range);
            //Gizmos.DrawWireCube(transform.position, Vector3.one * _range * 2);
        }

        void Update()
        {
            Viewable();
        }

        void Viewable()
        {
            // 시야각의 경계선
            Vector3 left = Angle(-_viewAngle * 0.5f);
            Vector3 right = Angle(_viewAngle * 0.5f);

            Debug.DrawRay(transform.position, left * _range, Color.green);
            Debug.DrawRay(transform.position, right * _range, Color.green);

            // 범위에 걸린 타겟들
            Collider[] targets = Physics.OverlapSphere(transform.position, _range, _targetMask);
            //Collider[] targets = Physics.OverlapBox(transform.position, Vector3.one * _range, Quaternion.identity, _targetMask);

            foreach (Collider target in targets)
            {
                Transform trTarget = target.transform;
                Vector3 dirTarget = (trTarget.position - transform.position).normalized;

                // 시야각에 걸리는지 확인
                if (Vector3.Angle(transform.forward, dirTarget) < _viewAngle / 2)
                {
                    float disTarget = Vector3.Distance(transform.position, trTarget.position);

                    // 장애물이 있는지 확인
                    if (!Physics.Raycast(transform.position, dirTarget, disTarget, _obstacleMask))
                    {
                        Debug.DrawRay(transform.position, dirTarget * disTarget, Color.red);
                    }
                }
            }
        }

        // Sphere 전용
        Vector3 Angle(float angle)
        {
            angle += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
        }
    }
}
