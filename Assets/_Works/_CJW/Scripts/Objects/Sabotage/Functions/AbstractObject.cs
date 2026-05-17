using System;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public abstract class AbstractObject : MonoBehaviour
    {
        [SerializeField] protected float lifeTime;
        [SerializeField] private EventChannelSO cameraEvent;
        private Action _onDestroy;
        protected virtual void Awake()
        {
            cameraEvent.AddListener<TopViewEvent>(OnTopViewEvent);
        }

        private void OnTopViewEvent(TopViewEvent topView)
        {
            if (!topView.IsTopView)
                HandleTopViewEnd();
        }
        public void SetLifetime(float time, Action onDestroy)
        {
            lifeTime = time;
            _onDestroy = onDestroy;
        } 
        protected virtual void HandleTopViewEnd()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnDestroy()
        {
            cameraEvent.RemoveListener<TopViewEvent>(OnTopViewEvent);

            _onDestroy?.Invoke();
        }

        //에너미가 콜라이더 범위에 들어왔을 때 실행할 함수
        protected abstract void OnTriggerEnterEnemy(AbstractEnemy enemy);
        // 에너미가 콜라이더 범위 밖으로 나갔을 때 실행할 함수
        protected abstract void OnTriggerExitEnemy(AbstractEnemy enemy);
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AbstractEnemy>(out var enemy))
            {
                OnTriggerEnterEnemy(enemy);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<AbstractEnemy>(out var enemy))
            {
                OnTriggerExitEnemy(enemy);
            }
        }
    }
}