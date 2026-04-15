using _Script.Agent.Modules;
using _Works._JYG._Script.Enemy;
using GameLib.PoolObject.Runtime;
using System;
using UnityEngine;

public class AttackModule : MonoBehaviour, IModule
{
    [field: SerializeField] public PoolManagerSO PoolManager { get; private set; }

    public event Action OnAttackEnd;

    private TargetRaycaster _targetRaycaster;
    public void Initialize(ModuleOwner moduleOwner)
    {
        _targetRaycaster = moduleOwner.GetModule<TargetRaycaster>();
    }




}
