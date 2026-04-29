using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using Agents.FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyAttackState : AgentState
    {
        private GameObject _player;
        public EnemyAttackState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _player = GameManager.Instance.Player;
        }

        public override void Enter()
        {
            base.Enter(); // _isTriggerCall = false;
            //공격 실행, OnAttackEnd 이벤트에 AnimationEndTrigger 넣어주기.
            Debug.Log("ATTACK!");
        }

        public override void Update()
        {
            Vector3 lookDir = _player.transform.position - _agent.transform.position;
            lookDir.y = 0;
            _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation
                , Quaternion.LookRotation(lookDir) // 적을 바라보는 벡터 구하기.
                , Time.deltaTime * 5f);

            //공격이 모두 끝났다면 다시 chase로 돌아간다.
            if(_isTriggerCall)
            {
                _enemy.ChangeState((int)EnemyState.CHASE);
            }
        }

        public override void Exit()
        {
            //OnAttackEnd 이벤트에서 AnimationEndTrigger 구독 취소.
            base.Exit();
        }
    }
}