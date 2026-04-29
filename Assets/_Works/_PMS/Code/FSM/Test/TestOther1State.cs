using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TestOther1State : State<GameObject>
{
    public override void Enter()
    {
        Debug.Log($"<color=green>Enter TestOther1State {Layer}</color>");
        Debug.Log("Please Press A");
    }

    public override void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
            Debug.Log("Input A");
    }

    public override void Exit()
    {
        Debug.Log($"<color=yellow>Exit TestOther1State {Layer}</color>");
    }

}
