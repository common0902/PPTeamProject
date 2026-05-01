using System;
using _Script.Agent;
using _Script.Agent.Modules;
using GameLib.PoolObject.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace _Works._JYG._Script.Enemy.CombatSystem
{
    public abstract class AbstractAttackModule : MonoBehaviour, IModule
    {
        public UnityEvent AttackFeedback;
        [field: SerializeField] public PoolManagerSO PoolManager { get; private set; }

        protected Agent agent;
        protected IAnimationTrigger trigger;
        public virtual void Initialize(ModuleOwner moduleOwner)
        {
            agent = moduleOwner as Agent;
            Debug.Assert(agent != null, $"AttackModule은 Agent가 필수적으로 필요합니다.");

            //agent.OnAttack += HandleAgentAttack;
            trigger = agent.GetModule<IAnimationTrigger>();
            Debug.Assert(trigger != null, $"Agent에 IAnimationTrigger가 존재하지 않습니다.");

            trigger.OnAttackTrigger += HandleAgentAttack;
        }

        protected virtual void OnDestroy()
        {
            //agent.OnAttack -= HandleAgentAttack;
            trigger.OnAttackTrigger -= HandleAgentAttack;
        }

        protected virtual void HandleAgentAttack()
        {
            AttackFeedback?.Invoke();
            //pooling
        }
    }
}
