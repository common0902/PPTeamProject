using System;
using System.Collections;
using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using _Works._JYG._Script.Enemy.FSM;
using _Works._JYG._Script.EventChannel.SystemEvent;
using Agents.FSM;
using UnityEngine;

namespace _Works._JYG._Script.Enemy
{
    public class AbstractEnemy : Agent
    {
        [Header("SO Settings")]
        [field: SerializeField] public EventChannelSO PlayerFindEventChannel { get; private set; }
        [field: SerializeField] protected StateListSO stateListSO { get; private set; }
        protected AgentStateMachine _stateMachine;

        [Header("Enemy Behaviour Settings")]
        public float enemyCurrentCaution;                       //에너미의 경계 수치. 1이 되면 위험 확정 상황.
        public float cautionRatio = 1f;                         //에너미의 경계 수치 증가값 배율.
        [SerializeField] private float enemyCautionDelay = 5f;  //위험까지 가기 위해 기다려야하는 시간초.

        [field: SerializeField] public float AttackDistance { get; private set; } = 15f;

        [field: SerializeField] public float PatrolSpeed { get; private set; } = 1.5f;    //Patrol 상태일 때 사용되는 걷는 속도
        [field: SerializeField] public float ChaseSpeed { get; private set; } = 2.5f;     //Chase 상태일 때 사용되는 뛰는 속도
        public float GetEnemyCaution => Mathf.Clamp01(enemyCurrentCaution / enemyCautionDelay); //0과 1로 표현하는 Enemy 경계수치
        public bool SirenEffect { get; private set; }

        [SerializeField] private float callingDuration = 3f;

        

        protected override void Awake()
        {
            base.Awake();
            PlayerFindEventChannel.AddListener<EnemyChangeState>(HandleEnemyChange);
        }

        protected override void Initialize()
        {
            base.Initialize();
            //PlayerFindEventChannel.AddListener();
        }

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlayerFindEventChannel.RemoveListener<EnemyChangeState>(HandleEnemyChange);
        }

        public void ChangeState(int index) => _stateMachine.ChangeState(index);
        public AgentState GetCurrentState => _stateMachine.CurrentState;

        public void EnemyFindPlayer()
        {
            if (enemyCurrentCaution >= 0)
                enemyCurrentCaution += Time.deltaTime * cautionRatio; //cautionRatio : Distance비례 증가값
            else
                enemyCurrentCaution = 0;
        }
        
        #region Enemy Siren Behaviour
        
        private void HandleEnemyChange(EnemyChangeState evt)
        {
            ChangeState((int)evt.NextState);
            enemyCurrentCaution = evt.NextState switch
            {
                EnemyState.CHASE => enemyCautionDelay,  //Chase상태라면, 사이렌이 울린거니까 에너미 경계치 최대로 상승
                EnemyState.PATROL => 0,                 //Patrol상태라면, 진정된거니 0으로 초기화.
                _ => enemyCurrentCaution                //나머지는 변함 없음.
            };

            if (evt.NextState == EnemyState.CHASE && !(_stateMachine.CurrentState is EnemyAttackState))
                SirenEffect = true;
            else if (evt.NextState == EnemyState.PATROL)
                SirenEffect = false;
        }
        
        public void SetSirenEffect(bool isEffected) => SirenEffect = isEffected;
        
        public void CallingPartner()
        {
            if (SirenEffect) return;
            
            StartCoroutine(StartCalling(callingDuration));
        }

        private IEnumerator StartCalling(float t)
        {
            yield return new WaitForSeconds(t);
            if (!IsDead && !SirenEffect)
            {
                PlayerFindEventChannel.RaiseEvent(PlayerFindEvents.EnemyChangeState.Init(EnemyState.CHASE));
            }
        }
        
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gold;
            Gizmos.DrawRay(transform.position, transform.forward.normalized * AttackDistance);
        }
    }
}