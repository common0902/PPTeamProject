using UnityEngine;

namespace _Works._JYG._Script.ScriptableObject
{
    [CreateAssetMenu(fileName = "new Bullet Enemy data", menuName = "Enemy/Bullet Enemy data", order = 0)]
    public class BulletEnemyData : UnityEngine.ScriptableObject
    {
        [field:SerializeField] public GameObject BulletPrefab { get; private set; }    //총알을 맞출 때 정확도를 의미함.
        [field:SerializeField] public float BulletSpeed { get; private set; }       //rigidbody velocity 기준.
        [field:SerializeField] public float BulletAccuracy { get; private set; }    //총알을 맞출 때 정확도를 의미함.
    }
}