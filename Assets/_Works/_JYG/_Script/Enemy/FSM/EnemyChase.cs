using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Works._JYG._Script.Enemy.PatrolSystem;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyChase : AgentState
    {
        private IAISystem _navmesh;
        private GameObject _player; //나중에 Player Component로 대체해야한다.
        public EnemyChase(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _navmesh = agent.GetModule<IAISystem>();
            _player = GameManager.Instance.Player;
        }
        public override void Enter()
        {
            base.Enter();
            _navmesh.Navmesh.isStopped = false;     
        }

        public override void Update()
        {
            base.Update();
            Ray ray = new Ray(_agent.transform.position, _agent.transform.position - _player.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20f))
            {
                //if(hit.transform.TryGetComponent<>)
            }
        }

        public override void Exit()
        {
            base.Exit();
            _navmesh.Navmesh.isStopped = true;
        }
    }
}