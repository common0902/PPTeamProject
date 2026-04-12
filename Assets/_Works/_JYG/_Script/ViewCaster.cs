using System;
using UnityEngine;

namespace _Works._JYG._Script
{
    public class ViewCaster : MonoBehaviour
    {
        private GameObject _player;
        
        [SerializeField] private float distance;
        [SerializeField] private float angle;

        private Vector3 _directionL;
        private Vector3 _directionR;

        private void Start()
        {
            _player = GameManager.Instance.Player;
            Debug.Assert(_player != null, $"Player를 찾을 수 없습니다.");

        }

        private void Update()
        {
            
            float ang = (angle + transform.parent.eulerAngles.y) *  Mathf.Deg2Rad;
            _directionL = new Vector3(Mathf.Cos(ang), 0,Mathf.Sin(ang));
            _directionR = new Vector3(Mathf.Cos(-ang), 0,Mathf.Sin(-ang));
            
            Debug.DrawRay(transform.position, _directionL.normalized * distance, Color.red);
            Debug.DrawRay(transform.position, _directionR.normalized * distance, Color.red);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }
}
