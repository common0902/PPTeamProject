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
        
        [SerializeField] private float distance;
        [SerializeField] private float angle;
        
        //public Action<

        private Vector3 _directionL;
        private Vector3 _directionR;
        
        
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
            if (_targetRaycaster.TryGetTarget())
            {
                if (Vector3.Distance(transform.position, _targetRaycaster.TargetPlayer.transform.position) <= distance) // 적이 +angle, -angle보다 안쪽 각도에 존재한다면, Action 실행
                {
                    if (Mathf.Abs(ang) <= angle / 2)
                    {
                        Debug.Log("Detected Enemy!");
                        Debug.Log("Enemy Angle is " + ang);
                    }
                }
                else
                {
                    Debug.Log("Can't Attach.");
                }
            }
            
            float radian = (angle + transform.parent.eulerAngles.y) * Mathf.Deg2Rad;
            _directionL = new Vector3(-Mathf.Cos(radian), 0,Mathf.Sin(radian)); //Gizmo로 그리기 위한 변수들임.
            _directionR = new Vector3(-Mathf.Cos(radian), 0,Mathf.Sin(-radian));
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, distance);
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, _directionL.normalized * distance, Color.red);
            Debug.DrawRay(transform.position, _directionR.normalized * distance, Color.red);
        }

    }
}
