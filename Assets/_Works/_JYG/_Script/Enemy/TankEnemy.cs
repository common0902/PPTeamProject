using UnityEngine;

namespace _Works._JYG._Script.Enemy
{
    public class TankEnemy : AbstractEnemy
    {
        [Header("Tank Setting")] 
        [SerializeField]
        private float shieldAngle = 60f;
        
        //Block Sound
        public override void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition)
        {
            if (Mathf.Acos(Vector3.Dot(transform.forward
                    , (attackerPosition - transform.position).normalized)) * Mathf.Rad2Deg
                < shieldAngle / 2)
            {
                //Block Sound Play
                Debug.Log("Block!!!!");
                return;
            }
            base.TakeDamage(damage, hitDirection, attackerPosition);
        }

        protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
        {
            base.HandleHealthChaged(prevHealth, currentHealth, max);
        }
    }
}
