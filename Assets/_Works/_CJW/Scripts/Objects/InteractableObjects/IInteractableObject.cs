namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    public interface IInteractableObject
    {
        
        bool IsPlayerInRange { get; }
        void HandleInteract();
        void SetFocused(bool focused);
    }
}