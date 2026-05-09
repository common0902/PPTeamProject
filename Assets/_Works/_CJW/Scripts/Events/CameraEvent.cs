using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Rendering;

namespace _Works._CJW.Scripts.Events
{
    public static class CameraEvent
    {
        public static readonly TopViewEvent TopViewEvent = new();
        public static readonly RegisterFovEvent RegisterFovEvent = new();
    }

    public class TopViewEvent : GameEvent
    {
        public bool IsTopView = false;

        public TopViewEvent Init(bool isTopView)
        {
            IsTopView = isTopView;
            return this;
        }
    }
    
    public class RegisterFovEvent : GameEvent
    {
        public bool IsRegistered; // 등록인지 해제인지
        public FOVRendering FovRendering; // 등록 또는 해제할 FOV 렌더링 컴포넌트

        public RegisterFovEvent Init(bool isRegistered, FOVRendering fovRendering)
        {
            IsRegistered = isRegistered;
            FovRendering = fovRendering;
            return this;
        }
    }
}