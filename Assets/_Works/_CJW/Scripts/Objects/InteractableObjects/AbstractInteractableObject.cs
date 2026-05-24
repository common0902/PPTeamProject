// AbstractInteractableObject.cs

using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractInteractableObject : MonoBehaviour, IInteractableObject
    {
        [field: SerializeField] public InteractableObjectDataSo DataSo { get; private set; }
        [SerializeField] private EventChannelSO soundChannel;
        [SerializeField] private SoundClipSO soundData;
        [field: SerializeField] public Transform UiShowPos { get; private set; }
        [SerializeField] private EventChannelSO interactEvent;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Color defaultColor; // 기본 아웃라인 컬러
        [SerializeField] private Color interactColor; // 상호작용 
        public bool IsPlayerInRange { get; private set; } = false; // 플레이어가 범위 내에 있는지
        private bool _isRegistered = false; // 오브젝트가 리스트에 등록되었는지
        private Outline _outline;
        public bool IsUsed { get; private set; }

        protected virtual void Awake()
        {
            _outline = GetComponent<Outline>();
            Debug.Assert(_outline != null, "Outline not found.");
            _outline.OutlineColor = defaultColor;
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어가 트리거 범위에 들면 관리 모듈에 등록함
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("HIT");
                if (_isRegistered || IsPlayerInRange || IsUsed) return;
                Debug.Log("HIT12312");
            
                _isRegistered = true;
                IsPlayerInRange = true;
                _outline.OutlineColor = interactColor;
                
                interactEvent.RaiseEvent(InteractEvents.ObjectRegisterEvent.Init(_isRegistered, this));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // 플레이어가 트리거 범위에서 나가면 관리 모듈에서 등록 해제
            if (other.gameObject.CompareTag("Player"))
            {
                if (!_isRegistered || !IsPlayerInRange) return;
            
                _isRegistered = false;
                IsPlayerInRange = false;
                _outline.OutlineColor = defaultColor;
                
                interactEvent.RaiseEvent(InteractEvents.ObjectRegisterEvent.Init(_isRegistered, this));
            }
        }

        public virtual void HandleInteract()
        {
            if (IsUsed) return;
            IsUsed = true;
            interactEvent.RaiseEvent(InteractEvents.InteractEvent); // 상호작용 이벤트 발생하도록
            soundChannel.RaiseEvent( SoundSystemEvents.PlaySoundEvent.Init(transform.position, soundData));
            // 상호작용 세부 구현은 자식에서
        }
        
        //플레이어와 가장 가까운 상호작용 오브젝트는 외곽선 표시
        public void SetFocused(bool focused)
        {
            if (focused)
                _outline.OutlineColor = interactColor;
            else
                _outline.OutlineColor = defaultColor;
        }

    }
}