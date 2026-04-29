using UnityEngine;
using UnityEngine.InputSystem;

public class TestDefaultState : State<GameObject>
{
    public override void Enter()
    {
        Debug.Log($"<color=green>Enter TestDefaultState {Layer}</color>");
        Debug.Log("Please Press SpaceBar");
    }

    public override void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            Debug.Log("Input Space");
    }

    public override void Exit()
    {
        Debug.Log($"<color=yellow>Exit TestDefaultState {Layer}</color>");
    }

}
