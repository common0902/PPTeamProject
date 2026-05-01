using _Script.Agent;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Agent
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

    protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
    {
        
    }
}
