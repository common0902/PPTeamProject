
public class PlayerAttackState : State<PlayerController>
{
    private WeaponModule _weaponModule;

    protected override void Setup()
    {
         _weaponModule = Entity.GetModule<WeaponModule>();
    }

    public override void Enter()
    {
        
        _weaponModule.Attack();
    }
}