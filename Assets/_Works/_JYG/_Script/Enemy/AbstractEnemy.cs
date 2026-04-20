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

        public float enemyCurrentCaution;                       //에너미의 경계 수치. 1이 되면 위험 확정 상황.
        public float cautionRatio = 1f;                         //에너미의 경계 수치 증가값 배율.
        [SerializeField] private float enemyCautionDelay = 5f;  //위험까지 가기 위해 기다려야하는 시간초.
        public float GetEnemyCaution => Mathf.Clamp01(enemyCurrentCaution / enemyCautionDelay);

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

        public void EnemyFindPlayer()
        {
            if (enemyCurrentCaution >= 0)
                enemyCurrentCaution += Time.deltaTime * cautionRatio;
            else
                enemyCurrentCaution = 0;
        }
    }
}