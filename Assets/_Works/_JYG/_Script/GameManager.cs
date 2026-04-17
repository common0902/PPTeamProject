using _Script.Tools.Utility;
using UnityEngine;

namespace _Works._JYG._Script
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoSingleton<GameManager>
    {
        [field: SerializeField] public GameObject Player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            //Application.targetFrameRate = 240; //프레임 고정하기
        }
    }
}
