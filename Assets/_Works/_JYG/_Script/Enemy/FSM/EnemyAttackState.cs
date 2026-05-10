using _Script.Agent;
using _Script.Agent.FSM;
using _Script.ScriptableObject;
using _Works._JYG._Script.Enemy.FSM.Tags;
using Agents.FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.FSM
{
    public class EnemyAttackState : AgentState
    {
        private GameObject _player;

        private TargetRaycaster _targetCaster;
        public EnemyAttackState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _player = GameManager.Instance.Player;

            _targetCaster = agent.GetModule<TargetRaycaster>();
        }

        public override void Enter()
        {
            base.Enter(); // _isTriggerCall = false;
            _enemy.SetCanRotate(false);
            _trigger.OnAnimationEnd += AnimationEndTrigger;
            _trigger.OnAnimationEnd += EnemyRotateReset;
        }

        private void EnemyRotateReset()
        {
            _enemy.SetCanRotate(true);
        }

        public override void Update()
        {
            base.Update();
            Vector3 lookDir = _player.transform.position - _agent.transform.position;
            lookDir.y = 0;
            _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation
                , Quaternion.LookRotation(lookDir) 
                , Time.deltaTime * _enemy.RotateSpeed);
            
            //공격이 모두 끝났다면 다시 chase로 돌아간다.
            if (_isTriggerCall 
                && (_targetCaster.TargetPlayer != null 
                || Vector3.Distance(_enemy.transform.position, _player.transform.position) > _enemy.AttackDistance))
            {
                if (!_targetCaster.TargetPlayer.TryGetComponent<TestPPPP>(out TestPPPP player) 
                    || Vector3.Distance(_enemy.transform.position, _player.transform.position) > _enemy.AttackDistance)
                {
                    _enemy.ChangeState((int)EnemyState.CHASE);
                    Debug.Log("Change State To Chase");
                }
            }
        }

        public override void Exit()
        {
            _trigger.OnAnimationEnd -= AnimationEndTrigger;
            _trigger.OnAnimationEnd -= EnemyRotateReset;
            base.Exit();
        }
    }
}