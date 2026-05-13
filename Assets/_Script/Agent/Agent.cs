using System;
using _Script.Agent.Modules;
using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Agent
{
    public abstract class Agent : ModuleOwner, IDamageable //Enemy와 User가 공통적으로 가지고 있는 요소들을 Agent로 묶어서 정의.
    {
        //Health System
        //Attack System (Skill)
        
        public UnityEvent OnHit;
        protected HealthModule Health { get; private set; }

        public bool IsDead { get; protected set; } = false;

        [field:SerializeField] public EventChannelSO SoundEventChannel { get; private set; }

        protected override void Awake()
        {
            base.Awake();   
        }

        protected override void Initialize() //이미 부모에서 GetModule을 할 조건이 갖추어져 있기 때문에 괜찮음.
        {
            base.Initialize();
            
            
            Health = GetModule<HealthModule>();

            Debug.Assert(Health != null, $"Agent {gameObject.name}가 HealthModule이 존재하지 않습니다!");
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            Health.OnHealthChanged += HandleHealthChaged;
            Health.OnDeath.AddListener(HandleIsDeathLogic);
        }

        private void HandleIsDeathLogic()
        {
            IsDead = true;
        }

        protected virtual void Update()
        {
        }

        protected virtual void OnDestroy()
        {
            if (Health != null)
            {
                Health.OnHealthChanged -= HandleHealthChaged;
                Health.OnDeath.RemoveListener(HandleIsDeathLogic);
            }
        }

        protected abstract void HandleHealthChaged(float prevHealth, float currentHealth, float max);
        
        public virtual void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition)
        {
            if (IsDead) return;
            
            Health.GetDamage(damage);
            OnHit?.Invoke();
        }

    }
}
