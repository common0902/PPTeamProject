using System;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Works._JTH.Scripts.UI.Event;
using _Works._JYG._Script.Enemy;
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
        [field: SerializeField] public EventChannelSO OpenUIEventChannel { get; private set; }

        [SerializeField] private GameObject enemyGroup;
        public int EnemyCount { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            //Application.targetFrameRate = 240; //프레임 고정하기
            if (enemyGroup != null)
                EnemyCount = enemyGroup.transform.childCount;
            else
                EnemyCount = 0;
        }

        [ContextMenu("EnemySiren")]
        public void PiiyongPPiyongEnemy()
        {
            PlayerFindEventChannel.RaiseEvent(PlayerFindEvents.EnemyChangeState.Init(EnemyState.CHASE));
        }

        public Vector3 GetPlayerMiddlePos()
        {
            Vector3 middle = Player.transform.position;
            middle.y += 1f;
            return middle;
        }

        public void EnemyDead()
        {
            EnemyCount--;
            if (EnemyCount <= 0)
            {
                OpenUIEventChannel.RaiseEvent(OpenUIEvents.OpenGameEndEvent.Init(false));
            }
        }
        
    }
}
