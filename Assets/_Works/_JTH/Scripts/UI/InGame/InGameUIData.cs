namespace _Works._JTH.Scripts.UI.InGame
{
    public struct InGameUIData
    {
        public enum WeaponType
        {
            Gun,
            Sword
        }

        public InGameUIData(int maxHp, float maxTopViewSkillCooldown, float maxSprintSkillCooldown, bool isGun)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;

            RemainingBullets = 0;

            CurrentWeapon = isGun ? WeaponType.Gun : WeaponType.Sword;
            
            MaxTopViewSkillCooldown = maxTopViewSkillCooldown;
            MaxSprintSkillCooldown = maxSprintSkillCooldown;
            
            RemainingSprintSkillCooldown = 0;
            RemainingTabSkillCooldown = 0;
        }
        
        public int MaxHp;
        public int CurrentHp;
        
        public int RemainingBullets;
        
        public WeaponType CurrentWeapon;
        
        public float RemainingTabSkillCooldown;
        public float RemainingSprintSkillCooldown;
        
        public float MaxTopViewSkillCooldown;
        public float MaxSprintSkillCooldown;
    }
}