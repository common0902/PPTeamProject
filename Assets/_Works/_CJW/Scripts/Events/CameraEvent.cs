using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Objects.InteractableObjects;
using _Works._CJW.Scripts.Objects.Sabotage;
using _Works._CJW.Scripts.Rendering;

namespace _Works._CJW.Scripts.Events
{
    public static class CameraEvent
    {
        public static readonly TopViewEvent TopViewEvent = new();
        public static readonly RegisterFovEvent RegisterFovEvent = new();
        public static readonly FocusedSabotageEvent FocusedSabotageEvent = new();
        public static readonly CameraElapseEvent CameraElapseEvent = new();
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

    public class CameraElapseEvent : GameEvent
    {
        public float Elapsed;

        public CameraElapseEvent Init(float elapse)
        {
            Elapsed = elapse;
            return this;
        }
    }
    public class FocusedSabotageEvent : GameEvent
    {
        public bool IsFocused;// true면 현재 포커스됨
        public Sabotage Sabotage; 
        
        public FocusedSabotageEvent Init(Sabotage sabotage, bool isFocused)
        {
            Sabotage = sabotage;
            IsFocused = isFocused;
            return this;
        }
    }
    public class RegisterSabotageEvent : GameEvent
    {
        public bool Register;// true면 현재 포커스됨
        public Sabotage Sabotage; 
        
        public RegisterSabotageEvent Init(Sabotage sabotage, bool register)
        {
            Sabotage = sabotage;
            Register = register;
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