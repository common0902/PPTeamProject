using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Objects;
using _Works._CJW.Scripts.Objects.InteractableObjects;

namespace _Works._CJW.Scripts.Events
{
    public static class InteractEvents
    {
        public static readonly InteractKeyEvent InteractKeyEvent = new();
        public static readonly ObjectRegisterEvent ObjectRegisterEvent = new();
        public static readonly InteractEvent InteractEvent = new();
    }
    
    // 상호작용 키 이벤트. 플레이어가 상호작용 키를 눌렀을 때 발생.
    // 매니저 모듈에서 이컬로 메서드 실행시킴
    // Agent 구현할거 너무 많아서 만든거니 지우고 모듈에서 인풋 리더 구독해서 사용해도 됨
    public class InteractKeyEvent : GameEvent 
    {
        public bool IsInteracted { get; private set; }

        public InteractKeyEvent Init(bool interact)
        {
            IsInteracted = interact;
            return this;
        }
    }

    // 등록 이벤트. 플레이어가 상호작용 범위에 들어오거나 나갈 때 발생.
    // 관리 모듈에서 이 이벤트를 받아서 리스트에 등록하거나 해제함
    public class ObjectRegisterEvent : GameEvent 
    {
        public bool IsRegistered { get; private set; }
        public IInteractableObject InteractableObject { get; private set; }
        
        public ObjectRegisterEvent Init(bool register, IInteractableObject interactableObject)
        {
            IsRegistered = register;
            InteractableObject = interactableObject;
            return this;
        }
    }
    
    public class InteractEvent : GameEvent // 상호작용 시 발동되는 이벤트
    {
    }
    
    // 해금 이벤트. 해금이 필요한 사보타지를 위해 이벤트 만들었음.
    public class UnlockEvent : GameEvent
    {
        public SabotageDataSo TargetSabotageData; // 해금이 필요한 사보타지 데이터. 이걸로 어떤 사보타지가 해금되었는지 구별 가능
        public UnlockEvent Init(SabotageDataSo targetSabotage)
        {
            TargetSabotageData = targetSabotage;
            return this;
        }
    }
}