using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Works._JYG._Script.Enemy.FSM.Tags;
using _Works._JYG._Script.Enemy.PatrolSystem;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyDeadState : AgentState
    {
        private IAISystem _aiSystem;
        private float _deleteTime = 3f;
        private float _currentTime;
        public EnemyDeadState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _aiSystem = agent.GetModule<IAISystem>();
        }

        public override void Enter()
        {
            base.Enter();
            _aiSystem.Navmesh.isStopped = true;     //이동 삭제
            _enemy.gameObject.GetComponent<Collider>().enabled = false;
            _currentTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            if (_currentTime + _deleteTime < Time.time)
            {
                _enemy.gameObject.SetActive(false);
            }
        }
    }
}
