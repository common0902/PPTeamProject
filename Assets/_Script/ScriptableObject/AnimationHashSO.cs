using UnityEngine;

namespace _Script.ScriptableObject
{
    [CreateAssetMenu(fileName = "New Hash SO", menuName = "Tools/Hash SO", order = 0)]
    public class AnimationHashSO : UnityEngine.ScriptableObject
    {
        [field:SerializeField] public string String { get; private set; }
        [field:SerializeField] public int AnimationHash { get; private set; }

        private void OnValidate()
        {
            AnimationHash = Animator.StringToHash(String);
        }
    }
}
