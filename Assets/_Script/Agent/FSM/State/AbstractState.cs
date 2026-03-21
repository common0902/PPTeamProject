using _Script.ScriptableObject;

namespace _Script.Agent.FSM.State
{
    public class AbstractState : AgentState
    {
        protected int clipHash;
        public AbstractState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            clipHash = hash.AnimationHash;
        }

        public override void Enter()
        {
            base.Enter();
            
            _renderer.PlayAnimation(clipHash);
        }
    }
}