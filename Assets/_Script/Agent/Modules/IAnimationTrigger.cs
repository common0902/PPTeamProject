using System;

namespace _Script.Agent.Modules
{
    public interface IAnimationTrigger
    {
        event Action OnAnimationEnd;
        event Action OnAttackTrigger;
    }
}