using _Script.Agent.Modules;
using _Script.ScriptableObject;
using _Works._JYG._Script;
using _Works._JYG._Script.Enemy;
using Agents.FSM;
using UnityEngine;

namespace _Script.Agent.FSM
{
    public class AgentState
    {
        protected Agent _agent;
        protected AbstractEnemy _enemy;
        protected bool _isTriggerCall;
        
        protected IRenderer _renderer;
        protected AnimationHashSO _animationHash;

        protected ViewCaster _viewCaster;

        public AgentState(Agent agent, AnimationHashSO hash) //ParamSO도 받아와야함.
        {
            _agent = agent;

            _enemy = agent as AbstractEnemy;
            
            _renderer = agent.GetModule<IRenderer>();
            
            _viewCaster = agent.GetModule<ViewCaster>();
            Debug.Assert(_viewCaster != null, $"Enemy에는 ViewCaster가 필수적으로 포함되어야 합니다!");
        }


        public virtual void Enter()
        {
            //_renderer.PlayCrossFade(_animationHash.AnimationHash, 0, 0);
            _isTriggerCall = false;
        }

        public virtual void Update()
        {
            //시야각 안에 들어오면 공격 감지하도록 만들어야 함.
            //IDLE, Patrol 상태에서만 View를 체크해야하고, 다른 State에서는 이미 유저의 정체가 탄로났기 때문에 base.Update()를 하지 않아야 한다.
            //시야각에 닿으면 IDLE로 넘어가고, 유저를 쳐다보아야 한다.
            //이미 IDLE이라면, Enemy의 경계수치를 높힌다.
        }

        public virtual void Exit()
        {
            
        }

        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}