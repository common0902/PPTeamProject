using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using Agents.FSM;
using UnityEngine;

namespace _Works._JYG._Script.Enemy
{
    public class AbstractEnemy : Agent
    {
        [field: SerializeField] protected StateListSO stateListSO { get; private set; }
        protected AgentStateMachine _stateMachine;

        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            if (stateListSO != null)
            {
                _stateMachine = new AgentStateMachine(this, stateListSO.states);
                ChangeState((int)EnemyState.PATROL);
            }
        }
        protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
        {

        }

        private void Update()
        {
            if (_stateMachine != null)
                _stateMachine.UpdateStateMachine();
        }
        public void ChangeState(int index) => _stateMachine.ChangeState(index);
        public AgentState GetCurrentState => _stateMachine.CurrentState;
    }
}