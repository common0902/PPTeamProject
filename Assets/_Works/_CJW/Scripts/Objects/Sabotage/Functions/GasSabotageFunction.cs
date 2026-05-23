using System;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class GasSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject gasObject;
        [SerializeField] private Transform[] spawnPos;
        [SerializeField] private Vector3 gasSize;
        [SerializeField] private float duration;

        public override void UseFunction()
        {
            base.UseFunction();
            if (spawnPos.Length < 1)
                return;
            foreach (Transform trm in spawnPos)
            {
                var gas = Instantiate(gasObject, trm.position, Quaternion.identity);
                gas.InitSize(gasSize);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (spawnPos == null) return;

            foreach (var spawnPo in spawnPos)
            {
                if (spawnPo == null) continue; 

                Gizmos.DrawCube(spawnPo.position, gasSize);
            }
        }
    }
}