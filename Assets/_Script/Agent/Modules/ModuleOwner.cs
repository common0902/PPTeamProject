using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Script.Agent.Modules
{
    public class ModuleOwner : MonoBehaviour
    {
        private Dictionary<Type, IModule> modules = new Dictionary<Type, IModule>();
        protected virtual void Awake()
        {
            modules = GetComponentsInChildren<IModule>().ToDictionary(m => m.GetType()); //자식에서 모든 IModule을 서칭 후, Dictionary에 등록
            Debug.Assert(modules != null, $"ModuleAgent {gameObject.name}의 모듈이 존재하지 않습니다.");
        
            Initialize();
            AfterInitialize();
        }

        /// <summary>
        /// ModuleAgent의 Awake에서 AfterInitialize가 호출되기 전에 실행됩니다.
        /// Module의 기본적인 구성이 필요할 때 사용됩니다.
        /// </summary>
        protected virtual void Initialize()
        {
            if (modules.Count == 0)
            {
                Debug.LogWarning($"ModuleAgent Warning \n{gameObject.name}의 모듈이 존재하지 않지만 ModuleAgent Component가 존재합니다.");
                return;
            }

            foreach (IModule module in modules.Values)
            {
                module.Initialize(this);
            }
        
        }
        /// <summary>
        /// ModuleAgent의 Awake에서 Initialize가 호출된 후에 실행됩니다.
        /// </summary>
        protected virtual void AfterInitialize()
        {
            if (modules.Count == 0) return;
            foreach (IAfterInitialize module in modules.Values.OfType<IAfterInitialize>()) // Dictionary들 중, ILateInitialize를 가지고 있는 애들만 선별한다. (OfType)
            {
                module.LateInitialize(this);
            }
        }

        /// <summary>
        /// ModuleAgent에 등록되어있는 [IModule]을 찾아서 반환합니다.
        /// </summary>
        /// <typeparam name="T">GetComponent처럼 가져오고싶은 Module을 입력하세요.</typeparam>
        /// <returns>T type(Module)을 return합니다.</returns>
        public T GetModule<T>()
        {
            if (modules.TryGetValue(typeof(T), out IModule module)) // Dictionary에서 해당하는 모듈이 있는지 Key를 넣어 찾는다. (GetComponent와 동일)
            {
                return (T)module;
            }
            IModule findFirstModule = modules.Values.FirstOrDefault(castedModule => castedModule is T); // 해당하는 Key가 없다. 그래서 Value를 확인해본다.
            if (findFirstModule is T) // Value와 일치한다면, 그 값을 되돌려준다.
            {
                return (T)findFirstModule;
            }
            return default(T); //없으면 null (default)를 Return해준다.
        }
    }
}
