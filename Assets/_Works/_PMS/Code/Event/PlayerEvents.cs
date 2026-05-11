using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._PMS.Code.Event
{
    public class PlayerEvents : MonoBehaviour
    {
        public static class OpenUIEvents
        {
            
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

        public class BulletChange : GameEvent
        {
            public int Bullet;

            public BulletChange Init(int bullet)
            {
                Bullet = bullet;
                return this;
            }
        }

        public class WeaponChange : GameEvent
        {
            public bool IsGun;

            public WeaponChange Init(bool isGun)
            {
                IsGun = isGun;
                return this;
            }
        }
    }
}