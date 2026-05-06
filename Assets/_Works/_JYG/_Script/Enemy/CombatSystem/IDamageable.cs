using UnityEngine;

namespace _Works._JYG._Script.Enemy.CombatSystem
{
    public interface IDamageable
    {
        /// <summary>
        /// Object가 데미지를 입을 때 사용하는 함수
        /// </summary>
        /// <param name="damage">입는 데미지 값</param>
        /// <param name="hitDirection">공격 시전자의 forward Vector</param>
        public void TakeDamage(float damage, Vector3 hitDirection, Vector3 attackerPosition);
    }
}
