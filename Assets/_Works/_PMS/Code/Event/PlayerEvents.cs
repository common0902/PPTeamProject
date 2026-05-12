using _Script.ScriptableObject.Event;

namespace _Works._PMS.Code.Event
{
    public static class PlayerEvents
    {
        public static SprintEndEvent SprintEndEvent = new SprintEndEvent();
        public static HitEvent HitEvent = new HitEvent();
        public static BulletChangeEvent BulletChangeEvent = new BulletChangeEvent();
        public static WeaponChangeEvent WeaponChangeEvent = new WeaponChangeEvent();
    }

    public class SprintEndEvent : GameEvent
    {
        
    }

    public class HitEvent : GameEvent
    {
        public float Hp;

        public HitEvent Init(float hp)
        {
            Hp = hp;
            return this;
        }
    }

    public class BulletChangeEvent : GameEvent
    {
        public int Bullet;

        public BulletChangeEvent Init(int bullet)
        {
            Bullet = bullet;
            return this;
        }
    }

    public class WeaponChangeEvent : GameEvent
    {
        public bool IsGun;

        public WeaponChangeEvent Init(bool isGun)
        {
            IsGun = isGun;
            return this;
        }
    }
}