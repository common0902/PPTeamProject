using System;
using _Script.Agent.Modules;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._JYG._Script
{
    public class ViewCaster : MonoBehaviour, IModule
    {
        private ModuleOwner _moduleOwner;

        private TargetRaycaster _targetRaycaster;
        
        private GameObject _player;
        
        [field:SerializeField] public float Distance { get; private set; }
        [SerializeField] private float angle;
        
        //public Action<

        private Vector3 _directionL;
        private Vector3 _directionR;

        public bool IsTargetAttached { get; private set; } = false; //적이 시야 범위 내에 들어왔는지를 감지함.


        public void Initialize(ModuleOwner moduleOwner)
        {
            _moduleOwner = moduleOwner;
            
            _player = GameManager.Instance.Player;
            Debug.Assert(_player != null, $"Player를 찾을 수 없습니다.");

            _targetRaycaster = moduleOwner.GetModule<TargetRaycaster>();
            Debug.Assert(_targetRaycaster != null, $"Target Raycaster가 존재하지 않습니다.");
        }

        private void Update()
        {
            float ang = Mathf.Acos(Vector3.Dot(transform.forward, (_player.transform.position - transform.position).normalized)) * Mathf.Rad2Deg;
            
            float radian = (angle + transform.parent.eulerAngles.y) * Mathf.Deg2Rad; //절대적으로 시야각만 표현해주기 위한 각도.
            _directionL = new Vector3(-Mathf.Cos(radian), 0,Mathf.Sin(radian)); //Gizmo로 그리기 위한 변수들임.
            _directionR = new Vector3(-Mathf.Cos(radian), 0,Mathf.Sin(-radian)); // 삼각함수 이용.
            
            
            if (_targetRaycaster.TryGetTarget())
            {
                //타겟 대상이 존재한다.
                if (Vector3.Distance(transform.position, _targetRaycaster.TargetPlayer.transform.position) <= Distance) // 적이 +angle, -angle보다 안쪽 각도에 존재한다면, Action 실행
                {
                    //정의한 거리 안에 타겟이 접근하였다.
                    if (Mathf.Abs(ang) <= angle / 2)
                    {
                        //Enemy가 시야각 안에 들어왔다.
                        IsTargetAttached = true;

                        return;
                    }
                }
            }
            //조건에 부합하지 않아 어쨌든 타겟이 접근하지 않았다.
            IsTargetAttached = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, Distance);
            
            Debug.DrawRay(transform.position, _directionL.normalized * Distance, Color.red);
            Debug.DrawRay(transform.position, _directionR.normalized * Distance, Color.red);
        }
    }
}
