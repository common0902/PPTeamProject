using System;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects
{
    public abstract class AbstractInteractableObject : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactRange;
        [SerializeField] private EventChannelSO interactEvent; // 플레이어가 인터렉트할거
        
        private bool _isPlayerInRange = false;
        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            Debug.Assert(_outline != null, "Outline component not found on the interactable object.");
            _outline.enabled = false;
        }

        protected virtual void HandleInteract(GameEvent evt)
        {
            _isPlayerInRange = false;
        }

        private void FixedUpdate()
        {
            var hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

            
            if(hits.Length > 0 
               && _isPlayerInRange == false)
            {
                HandleTriggerEnterEvent();
            }
            else if (hits.Length == 0 
                && _isPlayerInRange)
            {
                HandleTriggerExitEvent();
            }
        }

        protected virtual void HandleTriggerEnterEvent()
        {
            interactEvent.AddListener<GameEvent>(HandleInteract);
            _outline.enabled = true;
            _isPlayerInRange = true;
        } 
        protected virtual void HandleTriggerExitEvent()
        {
            interactEvent.RemoveListener<GameEvent>(HandleInteract);
            _outline.enabled = false;
            _isPlayerInRange = false;
        }

        private void OnDestroy()
        {
            interactEvent.RemoveListener<GameEvent>(HandleInteract);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }
    }
}
