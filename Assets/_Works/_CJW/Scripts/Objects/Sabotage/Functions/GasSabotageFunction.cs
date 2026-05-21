using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class GasSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject gasObject;
        [SerializeField] private Transform[] spawnPos;
        [SerializeField] private float duration;

        public override void UseFunction()
        {
            base.UseFunction();
            if (spawnPos.Length < 1)
                return;
            foreach (Transform trm in spawnPos)
            {
                Instantiate(gasObject, trm.position, Quaternion.identity);
            }
        }
    }
}