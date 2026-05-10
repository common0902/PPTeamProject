using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
{
    public event Action<Vector2> OnMovementChange;
    public event Action OnAttackKeyPressed;
    public event Action OnJumpKeyPressed;
    public event Action OnRunStarted;
    public event Action OnRunCanceled;
    public event Action OnViewMapStarted;
    public event Action OnViewMapCanceled;

    private Controls _controls;
    private bool _viewMap = false;

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
        if (context.started)
            OnRunStarted?.Invoke();
        else if (context.canceled)
            OnRunCanceled?.Invoke();
    }

    public void OnMap(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            if (!_viewMap)
            {
                OnViewMapStarted?.Invoke();
                _viewMap = true;
            }
            else
            {
                OnViewMapCanceled?.Invoke();
                _viewMap = false;
            }
        }
    }
}
