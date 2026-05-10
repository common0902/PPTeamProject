using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public abstract class AbstractSabotageFunctionModule : MonoBehaviour, IModule, ISabotageFunctionModule
    {
        private ModuleOwner _owner;
        public virtual void Initialize(ModuleOwner moduleOwner)
        {
            _owner = moduleOwner;
        }

        public abstract void UseFunction();
    }
}