using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class GasSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject gasObject;
        [SerializeField] private Transform spawnPos;

        public override void UseFunction()
        {
            Instantiate(gasObject, spawnPos.position, Quaternion.identity);
        }
    }
}