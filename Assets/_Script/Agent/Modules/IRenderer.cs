using _Script.ScriptableObject;

namespace _Script.Agent.Modules
{
    public interface IRenderer
    {
        void PlayAnimation(int hash, int layer = -1, float normalizedTime = 0);
        void SetBool(AnimationHashSO hash, bool value);
        void SetFloat(AnimationHashSO hash, float value);
        void SetInt(AnimationHashSO hash, int value);
        void SetTrigger(AnimationHashSO hash);
    }
}