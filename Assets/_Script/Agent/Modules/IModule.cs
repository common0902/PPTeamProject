namespace _Script.Agent.Modules
{
    public interface IModule
    {
        /// <summary>
        /// 클래스의 생성자처럼, 모듈에게는 생성자 Initialize가 존재합니다.
        /// 더 느린 생성자로 ILateInitialize가 존재합니다.
        /// </summary>
        /// <param name="moduleAgent">ModuleAgent를 받아옵니다. ModuleAgent에서 GetModule을 통해 모듈을 받아오세요. </param>
        void Initialize(ModuleOwner moduleAgent);
    }
}