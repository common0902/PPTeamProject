using System;
using System.Collections.Generic;
using System.Linq;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Objects;
using _Works._CJW.Scripts.Objects.InteractableObjects;
using _Works._CJW.Scripts.Objects.Sabotage.Functions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts
{
    public class PlayerInteractManageModule : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO interactEvent;
        
        private List<AbstractInteractableObject> _interactableObjects = new();
        private AbstractInteractableObject _currentObject;
        private PlayerController _owner;
        public void Initialize(ModuleOwner moduleOwner)
        {
            _owner = moduleOwner as PlayerController;
            // Debug.Assert(_owner != null, "PlayerInteractManageModule must be attached to a Player.");
            interactEvent.AddListener<ObjectRegisterEvent>(HandleRegister);

            _owner.PlayerInput.OnInteractKeyPressed += HandleInteractEvent;
        }

        

        // 상호작용 오브젝트 범위 내에 들었을 때 실행되는 핸들러
        // 실행시 주변 상호작용 오브젝트가 리스트에 등록되고 가까운 오브젝트를 찾음
        private void HandleRegister(ObjectRegisterEvent obj)
        {
            if (obj.IsRegistered)
            {
                if (!_interactableObjects.Contains(obj.InteractableObject))
                {
                    _interactableObjects.Add(obj.InteractableObject);
                }
            }
            else
            {
                if (_interactableObjects.Contains(obj.InteractableObject))
                {
                    _interactableObjects.Remove(obj.InteractableObject);
                }
            }
            UpdateFocused();
        }

        private void LateUpdate()
        {
            UpdateFocused();
        }

        private void HandleInteractEvent()
        {
            _currentObject?.HandleInteract();
        }

        private void UpdateFocused()
        {
            Debug.Log(_interactableObjects.Count);
            if(_interactableObjects.Count == 0) return;
            
            //가장 가까운 오브젝트와 상호작용 가능하게 하고, 시각적으로 표현함
            var nearObject = _interactableObjects.OrderBy
            (interactObject => Vector3.Distance(transform.position
                , interactObject.transform.position)).FirstOrDefault();
            
            if (_currentObject != nearObject && nearObject != null)
            {
                interactEvent.RaiseEvent(InteractEvents.ClosestObjectEvent
                    .Init(nearObject.DataSo, nearObject.transform.position));
            }
            
            _currentObject?.SetFocused(false);
            _currentObject = nearObject;
            _currentObject?.SetFocused(true);
            
        }

        private void OnDestroy()
        {
            _owner.PlayerInput.OnInteractKeyPressed -= HandleInteractEvent;

            interactEvent.RemoveListener<ObjectRegisterEvent>(HandleRegister);
        }
    }
}