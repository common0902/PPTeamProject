using UnityEngine;
using UnityEngine.InputSystem;

public class TestOther2State : State<GameObject>
{
    public override void Enter()
    {
        Debug.Log($"<color=green>Enter TestOther2State {Layer}</color>");
        Entity.GetComponent<Rigidbody>().useGravity = false;
        Debug.Log("Gravity Off");
        Debug.Log("Please Press S");
    }

    public override void Update()
    {
        Debug.Log(111111111111);
        if (Keyboard.current.sKey.isPressed)
        {
            Debug.Log(222222222222);
            Owner.ExecuteCommand(0);
        }
    }

    public override void Exit()
    {
        Entity.GetComponent<Rigidbody>().useGravity = true;
        Debug.Log("Gravity On");
        Debug.Log($"<color=yellow>Exit TestOther2State {Layer}</color>");
    }

}
