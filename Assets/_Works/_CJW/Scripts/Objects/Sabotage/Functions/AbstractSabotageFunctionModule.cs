using _Script.Agent.Modules;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public abstract class AbstractSabotageFunctionModule : MonoBehaviour, IModule, ISabotageFunctionModule
    {
        [SerializeField] private LayerMask groundLayer;
        
        private ModuleOwner _owner;
        public virtual void Initialize(ModuleOwner moduleOwner)
        {
            _owner = moduleOwner;
        }

        public abstract void UseFunction();
        
        protected bool GetGround(out RaycastHit hit)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundLayer))
            {
                return true;
            }

            return false;
        }
    }
}