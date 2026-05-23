using UnityEngine;

namespace _Works._JYG._Script.Enemy
{
    public class EnemyDeadSpaghetti : MonoBehaviour // 레전드 에너미가 죽을 때 스파게티로 작동하는 코드. Destroy
    {
        [SerializeField] private GameObject mySelf;

        public void EnemyDestroy()
        {
            Destroy(mySelf, 5f);
        }
    }
}
