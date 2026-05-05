using UnityEngine;

namespace _Works._JYG._Script.Enemy
{
    public class TankEnemy : AbstractEnemy
    {
        [Header("Tank Setting")] 
        [SerializeField]
        private float shieldAngle = 60f;
        
        //Block Sound
        public override void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (Vector3.Dot(transform.forward, hitDirection) > shieldAngle / 2)
            {
                //Block Sound Play
                return;
            }
            base.TakeDamage(damage, hitDirection);
        }

        protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
        {
            base.HandleHealthChaged(prevHealth, currentHealth, max);
        }
    }
}
