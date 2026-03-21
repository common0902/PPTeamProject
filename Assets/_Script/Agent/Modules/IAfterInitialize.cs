namespace _Script.Agent.Modules
{
    public interface IAfterInitialize
    {
        /// <summary>
        /// IModule의 Initialize보다 느리게 Initialize하는 Module입니다.
        /// </summary>
        /// <param name="moduleAgent">IModule과 동일. ModuleAgent를 받아옵니다. ModuleAgent에서 GetModule을 통해 모듈을 받아오세요.</param>
        void LateInitialize(ModuleOwner moduleAgent);
    }
}