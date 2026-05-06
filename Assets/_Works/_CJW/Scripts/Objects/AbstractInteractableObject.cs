// AbstractInteractableObject.cs
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractInteractableObject : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private EventChannelSO interactEvent;
        [SerializeField] private LayerMask playerLayer;
        public bool IsPlayerInRange { get; private set; } = false; // 플레이어가 범위 내에 있는지
        private bool _isRegistered = false; // 오브젝트가 리스트에 등록되었는지
        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            Debug.Assert(_outline != null, "Outline not found.");
            _outline.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어가 트리거 범위에 들면 관리 모듈에 등록함
            if ((playerLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (_isRegistered || IsPlayerInRange) return;
            
                Debug.Log("Player entered interaction range.");
                _isRegistered = true;
                IsPlayerInRange = true;
                interactEvent.RaiseEvent(InteractEvents.RegisterInteract.Init(_isRegistered, this));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // 플레이어가 트리거 범위에서 나가면 관리 모듈에서 등록 해제
            if ((playerLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (!_isRegistered || !IsPlayerInRange) return;
            
                _isRegistered = false;
                IsPlayerInRange = false;
                interactEvent.RaiseEvent(InteractEvents.RegisterInteract.Init(_isRegistered, this));
            }
        }

        public virtual void HandleInteract()
        {
            // 상호작용 세부 구현은 자식에서
        }
        
        //플레이어와 가장 가까운 상호작용 오브젝트는 외곽선 표시
        public void SetFocused(bool focused)
        {
            _outline.enabled = focused;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var col = GetComponent<SphereCollider>();
            if (col != null)
                Gizmos.DrawWireSphere(transform.position, col.radius);
        }
    }
}