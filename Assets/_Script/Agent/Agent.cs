using System;
using _Script.Agent.Modules;
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
        public UnityEvent OnDeath;

        public bool IsDead { get; protected set; }
        public bool IsHit { get; private set; }
        public bool IsHitFinished { get; private set; }

        [SerializeField] private float hitDuration = 0.5f; // Inspector에서 조절 가능
        private float _hitTimer;

        protected HealthModule Health { get; private set; }

        public Action OnAttack;

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
        }

        protected virtual void Update()
        {
            if (!IsHit) return;

            _hitTimer -= Time.deltaTime;

            if (_hitTimer <= 0f)
            {
                IsHit = false;
                IsHitFinished = true;
            }
            else
            {
                IsHitFinished = false;
            }
        }

        protected virtual void OnDestroy()
        {
            if(Health != null)
                Health.OnHealthChanged -= HandleHealthChaged;
        }

        protected abstract void HandleHealthChaged(float prevHealth, float currentHealth, float max);
        
        public virtual void TakeDamage(float damage, Vector3 hitDirection)
        {
            Health.GetDamage(damage);
            IsHit = true;
            IsHitFinished = false;
            _hitTimer = hitDuration;
            OnHit?.Invoke();
        }

        // IsHit 해제하는 메서드
        public void ClearHit() => IsHit = false;
    }
}
