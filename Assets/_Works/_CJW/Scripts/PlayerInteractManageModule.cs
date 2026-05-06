using System.Collections.Generic;
using System.Linq;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects;
using UnityEngine;

namespace _Works._CJW.Scripts
{
    public class PlayerInteractManageModule : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO interactEvent;
        
        private List<IInteractableObject> _interactableObjects = new();
        private IInteractableObject _currentObject;
        private Player _owner;
        public void Initialize(ModuleOwner moduleOwner)
        {
            _owner = moduleOwner as Player;
            // Debug.Assert(_owner != null, "PlayerInteractManageModule must be attached to a Player.");
            interactEvent.AddListener<InteractEvent>(HandleInteractEvent);
            interactEvent.AddListener<RegisterInteract>(HandleRegister);
        }

        // 상호작용 오브젝트 범위 내에 들었을 때 실행되는 핸들러
        // 실행시 주변 상호작용 오브젝트가 리스트에 등록되고 가까운 오브젝트를 찾음
        private void HandleRegister(RegisterInteract obj)
        {
            if (obj.IsRegistered)
            {
                if (!_interactableObjects.Contains(obj.InteractableObject))
                {
                    _interactableObjects.Add(obj.InteractableObject);
                    Debug.Log($"Registered object");
                }
            }
            else
            {
                if (_interactableObjects.Contains(obj.InteractableObject))
                {
                    _interactableObjects.Remove(obj.InteractableObject);
                    Debug.Log($"Unregistered object");
                }
            }
            UpdateFocused();
        }

        private void HandleInteractEvent(InteractEvent obj)
        {
            _currentObject?.HandleInteract();
        }

        private void UpdateFocused()
        {
            _currentObject?.SetFocused(false);
            if(_interactableObjects.Count == 0) return;
            
            var nearObject = _interactableObjects.OrderBy
            (interactObject => Vector3.Distance(transform.position
                , ((MonoBehaviour)interactObject).transform.position)).FirstOrDefault();
            _currentObject = nearObject;
            _currentObject?.SetFocused(true);
            
        }
    }
}