using System.Collections;
using System.Collections.Generic;
using _Script.Agent;
using _Script.Agent.FSM;
using _Script.Agent.Modules;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._CJW.Scripts.Rendering;
using _Works._JYG._Script.Enemy.FSM;
using _Works._JYG._Script.Enemy.FSM.Tags;
using _Works._JYG._Script.Enemy.PatrolSystem;
using _Works._JYG._Script.EventChannel.SystemEvent;
using Agents.FSM;
using GameLib.SoundSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Works._JYG._Script.Enemy
{
    public class AbstractEnemy : Agent
    {
        
        [Header("SO Settings")]
        [field: SerializeField] public EventChannelSO PlayerFindEventChannel { get; private set; }
        [field: SerializeField] public EventChannelSO SabotageEventChannel { get; private set; }
        [field: SerializeField] public EventChannelSO CameraEventChannel { get; private set; }
        public UnityEvent OnWalking { get; private set; }
        [field: SerializeField] protected StateListSO stateListSO { get; private set; }
        protected AgentStateMachine _stateMachine;

        [Header("Enemy Behaviour Settings")]
        public float enemyCurrentCaution;                       //에너미의 경계 수치. 1이 되면 위험 확정 상황.
        public float cautionRatio = 1f;                         //에너미의 경계 수치 증가값 배율.
        [SerializeField] private float enemyCautionDelay = 5f;  //위험까지 가기 위해 기다려야하는 시간초.

        [field: SerializeField] public float AttackDistance { get; private set; } = 15f;

        [field: SerializeField] public float PatrolSpeed { get; private set; } = 1.5f;    //Patrol 상태일 때 사용되는 걷는 속도
        [field: SerializeField] public float WaterSpeed { get; private set; } = 0.5f;    //Patrol 상태일 때 사용되는 걷는 속도
        [field: SerializeField] public float ChaseSpeed { get; private set; } = 2.5f;     //Chase 상태일 때 사용되는 뛰는 속도
        [field: SerializeField] public float RotateSpeed { get; private set; } = 5f;     //Chase 상태일 때 사용되는 뛰는 속도
        public float GetEnemyCaution => Mathf.Clamp01(enemyCurrentCaution / enemyCautionDelay); //0과 1로 표현하는 Enemy 경계수치
        public bool SirenEffect { get; private set; }
        public bool CanRotate { get; private set; }

        [SerializeField] private float callingDuration = 3f;

        [field: SerializeField] public SoundClipSO PlayerFoundSound { get; private set; }
        [field: SerializeField] public SoundClipSO HitSound { get; private set; }
        
        [field: SerializeField] public AnimationHashSO ForceXParam { get; private set; }
        [field: SerializeField] public AnimationHashSO ForceYParam { get; private set; }

        private FOVRendering _fovRenderer;

        private IRenderer _renderer;
        public IAISystem AiSystem { get; private set; }

        [field: SerializeField] public bool IsRunning { get; private set; } = true;
        public bool Calling { get; private set; } //이거 리팩토리할때 바꿔줘야함. 레전드 스파게티

        public bool isWater = false;

        public Outline EnemyOutline { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            PlayerFindEventChannel.AddListener<EnemyChangeState>(HandleEnemyChange);
            _fovRenderer = GetComponentInChildren<FOVRendering>();
            EnemyOutline = GetComponentInChildren<Outline>();
        }

        private void Start()
        {
            ViewCaster view = GetModule<ViewCaster>();
            _fovRenderer.angle = view.Angle;
            _fovRenderer.distance = view.Distance;
            
            _renderer = GetModule<IRenderer>();
            AiSystem = GetModule<IAISystem>();
            
            CameraEventChannel.AddListener<TopViewEvent>(HandleCameraTopViewEvent);
        }

        private void HandleCameraTopViewEvent(TopViewEvent obj)
        {
            _renderer.Animator.speed = obj.IsTopView ? 0 : 1;
            IsRunning = !obj.IsTopView;
            if (GetCurrentState is ICanMove)
            {
                AiSystem.Navmesh.isStopped = obj.IsTopView;
            }
        }

        public void ChangeWaterState(bool isOnWater)
        {
            isWater = isOnWater;
            if (isOnWater)
            {
                AiSystem.Navmesh.speed = WaterSpeed;
                return;
            }

            AiSystem.Navmesh.speed = GetCurrentState switch
            {
                EnemyPatrolState => PatrolSpeed,
                EnemyChaseState => ChaseSpeed,
                _ => 0
            };
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
            if (currentHealth <= 0)
            {
                GameManager.Instance.EnemyDead();
                ChangeState((int)EnemyState.Dead);
            }
        }

        protected override void Update()
        {
            if (_stateMachine != null)
                _stateMachine.UpdateStateMachine();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlayerFindEventChannel.RemoveListener<EnemyChangeState>(HandleEnemyChange);
            CameraEventChannel.RemoveListener<TopViewEvent>(HandleCameraTopViewEvent);
            
        }

        public void ChangeState(int index) => _stateMachine.ChangeState(index);
        public AgentState GetCurrentState => _stateMachine.CurrentState;
        public void SetCanRotate(bool canRotate) => CanRotate = canRotate;

        public override void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition)
        {
            SoundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, HitSound));
            
            //진짜 레전드 스파게티 코딩이네.
            Vector3 dumpingDirection = (attackerPosition - transform.position);
            
            if(Mathf.Abs(dumpingDirection.z) > Mathf.Abs(dumpingDirection.x))
                dumpingDirection.x = 0;
            else
                dumpingDirection.z = 0;
            dumpingDirection.Normalize();
            
            _renderer.SetFloat(ForceXParam, dumpingDirection.x);
            _renderer.SetFloat(ForceYParam, dumpingDirection.z);
            base.TakeDamage(damage, hitDirection, attackerPosition);
        }

        public void EnemyFindPlayer()
        {
            if (!IsRunning) return;
            if (enemyCurrentCaution >= 0)
                enemyCurrentCaution += Time.deltaTime * cautionRatio; //cautionRatio : Distance비례 증가값
            else
                enemyCurrentCaution = 0;
        }
        
        #region Enemy Siren Behaviour
        
        private void HandleEnemyChange(EnemyChangeState evt)
        {
            if (IsDead) return;
            
            if (evt.NextState == EnemyState.CHASE  && _stateMachine.CurrentState is not EnemyAttackState)
                SirenEffect = true;
            else if (evt.NextState == EnemyState.PATROL)
                SirenEffect = false;
            
            ChangeState((int)evt.NextState);
            
            enemyCurrentCaution = evt.NextState switch
            {
                EnemyState.CHASE => enemyCautionDelay,  //Chase상태라면, 사이렌이 울린거니까 에너미 경계치 최대로 상승
                EnemyState.PATROL => 0,                 //Patrol상태라면, 진정된거니 0으로 초기화.
                _ => enemyCurrentCaution                //나머지는 변함 없음.
            };
        }
        
        public void SetSirenEffect(bool isEffected) => SirenEffect = isEffected;
        
        public void CallingPartner()
        {
            if (SirenEffect || IsDead) return;
            
            StartCoroutine(StartCalling(callingDuration));
            Calling = true;
        }

        private IEnumerator StartCalling(float t)
        {
            if (!SirenEffect)
            {
                SoundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, PlayerFoundSound));
            }
            WaitUntil waitUntil = new WaitUntil(() => IsRunning);
            float curT = 0;
            while (curT < t)
            {
                curT += Time.deltaTime;
                yield return null;
                yield return waitUntil;
            }
            if (!IsDead && !SirenEffect)
            {
                PlayerFindEventChannel.RaiseEvent(PlayerFindEvents.EnemyChangeState.Init(EnemyState.CHASE));
                PlayerFindEventChannel.RaiseEvent(PlayerFindEvents.SirenCameraEffect);
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