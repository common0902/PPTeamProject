using _Script.ScriptableObject.Event;
using Agents.FSM;

namespace _Works._JYG._Script.EventChannel.SystemEvent
{
    public static class PlayerFindEvents
    {
        public static readonly EnemyChangeState EnemyChangeState = new EnemyChangeState();
    }
    public class EnemyChangeState : GameEvent
    {
        public EnemyState NextState { get; private set; }

        public EnemyChangeState Init(EnemyState nextState)
        {
            NextState = nextState;
            return this;
        }
    }
}