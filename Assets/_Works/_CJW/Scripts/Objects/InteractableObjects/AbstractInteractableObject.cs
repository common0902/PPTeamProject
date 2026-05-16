// AbstractInteractableObject.cs

using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractInteractableObject : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private EventChannelSO interactEvent;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Color defaultColor; // 기본 아웃라인 컬러
        [SerializeField] private Color interactColor; // 상호작용 
        [Header("안넣어도 문제는 없음 사보타지와 연결된거면 됨")]
        [SerializeField] private SabotageDataSo sabotageData; // 사보타지 데이터. 사보타지가 필요한 오브젝트는 이걸로 구별할 수 있음
        public bool IsPlayerInRange { get; private set; } = false; // 플레이어가 범위 내에 있는지
        private bool _isRegistered = false; // 오브젝트가 리스트에 등록되었는지
        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            Debug.Assert(_outline != null, "Outline not found.");
            _outline.OutlineColor = defaultColor;
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
                interactEvent.RaiseEvent(InteractEvents.ObjectRegisterEvent.Init(_isRegistered, this));
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
                interactEvent.RaiseEvent(InteractEvents.ObjectRegisterEvent.Init(_isRegistered, this));
            }
        }

        [ContextMenu("Interact")]
        public virtual void HandleInteract()
        {
            interactEvent.RaiseEvent(InteractEvents.InteractEvent); // 상호작용 이벤트 발생하도록
            if(sabotageData != null)
                interactEvent.RaiseEvent(new UnlockEvent().Init(sabotageData)); // 상호작용하면 본인의 사보타지 데이터를 보내고 해금 이벤트 발생하도록
            // 상호작용 세부 구현은 자식에서
        }
        
        //플레이어와 가장 가까운 상호작용 오브젝트는 외곽선 표시
        public void SetFocused(bool focused)
        {
            if (focused)
                _outline.OutlineColor = interactColor;
            else
                _outline.OutlineColor = defaultColor;
            Debug.Log(focused);
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