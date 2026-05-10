using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Works._JYG._Script.EventChannel.SystemEvent;
using Agents.FSM;
using GameLib.PoolObject.Runtime;
using UnityEngine;

namespace _Works._JYG._Script
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoSingleton<GameManager>
    {
        [field: SerializeField] public GameObject Player { get; private set; }
        [field:SerializeField] public PoolInitializer PoolInitializer { get; private set; }
        [field: SerializeField] public EventChannelSO PlayerFindEventChannel { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            //Application.targetFrameRate = 240; //프레임 고정하기
        }

        [ContextMenu("EnemySiren")]
        public void PiiyongPPiyongEnemy()
        {
            PlayerFindEventChannel.RaiseEvent(PlayerFindEvents.EnemyChangeState.Init(EnemyState.CHASE));
        }
        
    }
}
