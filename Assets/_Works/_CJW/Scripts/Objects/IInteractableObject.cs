namespace _Works._CJW.Scripts.Objects
{
    public interface IInteractableObject
    {
        bool IsPlayerInRange { get; }
        void HandleInteract();
    }
}