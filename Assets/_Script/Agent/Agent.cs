using System;
using _Script.Agent.FSM;
using _Script.Agent.Modules;
using _Script.ScriptableObject;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Agent
{
    public abstract class Agent : ModuleOwner //Enemy와 User가 공통적으로 가지고 있는 요소들을 Agent로 묶어서 정의.
    {
        //Health System
        //Attack System (Skill)
        
        public UnityEvent OnHit;
        public UnityEvent OnDeath;

        protected AgentStateMachine _stateMachine;
        [SerializeField] protected StateListSO stateList;

        public bool IsDead { get; protected set; }
        protected HealthModule Health { get; private set; }

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
            
            if(_stateMachine == null)
                _stateMachine = new AgentStateMachine(this, stateList.states);
            Health.OnHealthChanged += HandleHealthChaged;
        }

        

        private void OnDestroy()
        {
            if(Health != null)
                Health.OnHealthChanged -= HandleHealthChaged;
        }

        protected abstract void HandleHealthChaged(float prevHealth, float currentHealth, float max);
        
        private void Update()
        {
            if (_stateMachine == null)
            {
                _stateMachine = new AgentStateMachine(this, stateList.states);
            }
            _stateMachine.UpdateStateMachine();
        }
        public AgentState GetCurrentState() => _stateMachine.CurrentState;
        public void ChangeState(int nextState) => _stateMachine.ChangeState(nextState);
    }
}
