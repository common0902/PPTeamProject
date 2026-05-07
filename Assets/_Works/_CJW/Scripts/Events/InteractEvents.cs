using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Objects;

namespace _Works._CJW.Scripts.Events
{
    public static class InteractEvents
    {
        public static readonly InteractEvent InteractEvent = new();
        public static readonly RegisterInteract RegisterInteract = new();
        public static readonly UnlockEvent UnlockEvent = new();
    }
    public class InteractEvent : GameEvent // 상호작용 키 이벤트. 이거 없애도 될듯
    {
        public bool IsInteracted { get; private set; }

        public InteractEvent Init(bool interact)
        {
            IsInteracted = interact;
            return this;
        }
    }

    // 등록 이벤트. 플레이어가 상호작용 범위에 들어오거나 나갈 때 발생.
    // 관리 모듈에서 이 이벤트를 받아서 리스트에 등록하거나 해제함
    public class RegisterInteract : GameEvent 
    {
        public bool IsRegistered { get; private set; }
        public IInteractableObject InteractableObject { get; private set; }
        
        public RegisterInteract Init(bool register, IInteractableObject interactableObject)
        {
            IsRegistered = register;
            InteractableObject = interactableObject;
            return this;
        }
    }
    
    // 해금 이벤트. 해금이 필요한 사보타지를 위해 이벤트 만들었음.
    public class UnlockEvent : GameEvent
    {
        public bool IsUnlocked { get; private set; }

        public UnlockEvent Init(bool unlock)
        {
            IsUnlocked = unlock;
            return this;
        }
    }
}