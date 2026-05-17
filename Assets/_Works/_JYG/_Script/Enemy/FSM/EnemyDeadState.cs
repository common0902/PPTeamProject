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
        public EnemyDeadState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _aiSystem = agent.GetModule<IAISystem>();
        }

        public override void Enter()
        {
            base.Enter();
            _aiSystem.Navmesh.isStopped = true;     //이동 삭제
        }
    }
}
