using UnityEngine;

namespace _Script.Agent.Modules
{
    public class HealthModule : MonoBehaviour, IModule, IAfterInitialize
    {
        public delegate void HealthChanged(float prevHealth, float currentHealth, float max); //순서 조심하기. Health가 Change 된 경우 발생하는 Event
        public event HealthChanged OnHealthChanged;
        
        private float _currentHealth;
        private ModuleOwner _moduleAgent;

        [field: SerializeField] public float MaxHealth { get; private set; } = 2000f;

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                float before = _currentHealth;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                if (!Mathf.Approximately(_currentHealth, before))
                {
                    OnHealthChanged?.Invoke(before, _currentHealth, MaxHealth);
                }
            }
        }

        public void Initialize(ModuleOwner moduleOwner)
        {
            _moduleAgent = moduleOwner;
        }

        public void LateInitialize(ModuleOwner moduleAgent)
        {
            _currentHealth = MaxHealth;
        }

        public void GetDamage(float damage)
        {
            CurrentHealth -= damage;
        }

        public void ResetHealth()
        {
            _currentHealth = MaxHealth;
        }
    }
}