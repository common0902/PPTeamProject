using _Script.Agent.Modules;
using _Works._JYG._Script.ScriptableObject;
using UnityEngine;

namespace _Works._JYG._Script.Enemy.CombatSystem
{
    public class BulletEnemyAttackModule : AbstractAttackModule //총알을 쏘는 Gunner, Sniper Enemy의 AttackModule이다.
    {
        [field:SerializeField] public Transform FirePosition { get; private set; }
        [field: SerializeField] public BulletEnemyData BulletData { get; private set; }
        private Transform _playerTrm;

        public override void Initialize(ModuleOwner moduleOwner)
        {
            base.Initialize(moduleOwner);
            
            _playerTrm = GameManager.Instance.Player.transform;
        }

        protected override void HandleAgentAttack()
        {
            Debug.Log("Enemy Shooting!!!!!!!");
            base.HandleAgentAttack();
            GameObject bullet = Instantiate(BulletData.BulletPrefab, FirePosition.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().linearVelocity = (FirePosition.position - _playerTrm.position).normalized * BulletData.BulletSpeed;
        }
    }
}
