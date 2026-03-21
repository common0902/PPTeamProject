using System.Runtime.InteropServices;
using _Script.Agent.Modules;
using _Script.ScriptableObject;

namespace _Script.Agent.FSM
{
    public class AgentState
    {
        protected Agent _agent;
        protected bool _isTriggerCall;
        
        protected IRenderer _renderer;

        public AgentState(Agent agent, AnimationHashSO hash) //ParamSO도 받아와야함.
        {
            _agent = agent;
            
            _renderer = agent.GetModule<IRenderer>();
        }


        public virtual void Enter()
        {
            
        }

        public virtual void Update()
        {
            _isTriggerCall = false;
        }

        public virtual void Exit()
        {
            
        }

        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}