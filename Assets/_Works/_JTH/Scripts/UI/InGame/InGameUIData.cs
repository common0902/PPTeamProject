namespace _Works._JTH.Scripts.UI.InGame
{
    public struct InGameUIData
    {
        public enum WeaponType
        {
            Gun,
            Sword
        }

        public InGameUIData(int maxHp, float maxQSkillCooldown, float maxTabSkillCooldown, float maxShiftSkillCooldown)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;

            RemainingBullets = 0;

            CurrentWeapon = WeaponType.Gun;
            
            MaxQSkillCooldown = maxQSkillCooldown;
            MaxTabSkillCooldown = maxTabSkillCooldown;
            MaxShiftSkillCooldown = maxShiftSkillCooldown;
            
            RemainingShiftSkillCooldown = 0;
            RemainingTabSkillCooldown = 0;
            RemainingQSkillCooldown = 0;
        }
        
        public int MaxHp;
        public int CurrentHp;
        
        public int RemainingBullets;
        
        public WeaponType CurrentWeapon;
        
        public float RemainingQSkillCooldown;
        public float RemainingTabSkillCooldown;
        public float RemainingShiftSkillCooldown;
        
        public float MaxQSkillCooldown;
        public float MaxTabSkillCooldown;
        public float MaxShiftSkillCooldown;
    }
}