using _Works._CJW.Scripts;
using UnityEngine;

public class PlayerViewMapState : State<PlayerController>
{
    public override void Enter()
    {
        Entity.CamController.TransToTopView();
        Entity.Visual.SetActive(true);
        Entity.Weapons.SetActive(false);

        Entity.CamController.OnFirstViewComplete += OnFirstViewComplete;
    }

    public override void Exit()
    {
        Entity.CamController.TransToFirstView();
        Entity.Visual.SetActive(false); 
    }

    private void OnFirstViewComplete()
    {
        Entity.CamController.OnFirstViewComplete -= OnFirstViewComplete;
        Entity.Weapons.SetActive(true);
    }
}