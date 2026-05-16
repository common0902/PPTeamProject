using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class SprinklerSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject puddleObject;
        public override void UseFunction()
        {
            PlayParticle();
            
        }
    }
}