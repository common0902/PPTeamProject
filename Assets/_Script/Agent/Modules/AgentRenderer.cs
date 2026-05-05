using System;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.Modules
{
    [RequireComponent(typeof(Animator))]
    public class AgentRenderer : MonoBehaviour, IModule, IRenderer, IAnimationTrigger
    {
        private Agent _agent;
        private Animator _animator;
        public void Initialize(ModuleOwner moduleOwner)
        {
            _agent = moduleOwner as Agent;
            _animator = GetComponent<Animator>();
            
        }

        private void SetAgentAngle(Vector3 newAngle)
        {
            transform.eulerAngles = newAngle;
        }

        public void PlayAnimationWithSO(AnimationHashSO animationHash, float fadeDuration) =>
            PlayCrossFade(animationHash.AnimationHash, 0, fadeDuration);
        
        public void PlayAnimation(int hash, int layer = -1, float normalizedTime = 0) => _animator.Play(hash, layer, normalizedTime);

        public void PlayCrossFade(int clipHash, float normalizedTime, float fadeDuration, int layer = 0)
            => _animator.CrossFadeInFixedTime(clipHash, fadeDuration, layer, normalizedTime);
        public void SetBool(AnimationHashSO hash, bool value) => _animator.SetBool(hash.AnimationHash, value);
        public void SetFloat(AnimationHashSO hash, float value) => _animator.SetFloat(hash.AnimationHash, value);
        public void SetInt(AnimationHashSO hash, int value) => _animator.SetInteger(hash.AnimationHash, value);
        public void SetTrigger(AnimationHashSO hash) => _animator.SetTrigger(hash.AnimationHash);
        
        #region Trigger

        public event Action OnAnimationEnd;
        public event Action OnAttackTrigger;
        
        private void EndTrigger() => OnAnimationEnd?.Invoke();
        private void AttackTrigger() => OnAttackTrigger?.Invoke();

        #endregion
    }
}