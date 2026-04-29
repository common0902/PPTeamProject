using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
{
    public event Action<Vector2> OnMovementChange;
    public event Action OnAttackKeyPressed;
    public event Action OnJumpKeyPressed;
    public event Action OnSitKeyPressed;
    public event Action OnRunKeyPressed;

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        OnMovementChange?.Invoke(movement);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackKeyPressed?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpKeyPressed?.Invoke();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnRunKeyPressed?.Invoke();
    }

    public void OnSit(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSitKeyPressed?.Invoke();
    }
}
