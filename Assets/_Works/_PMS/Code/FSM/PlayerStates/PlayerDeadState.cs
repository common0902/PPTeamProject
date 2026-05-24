using _Works._JTH.Scripts.UI.Event;
using UnityEngine;

public class PlayerDeadState : State<PlayerController>
{
    public override void Enter()
    {
        base.Enter();
        Entity.OpenUIEventChannel.RaiseEvent(OpenUIEvents.OpenGameEndEvent.Init(true));
    }
}

