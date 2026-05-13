using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class SprinklerSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private GameObject puddleObject;
        [SerializeField] private Vector3 spawnOffset;
        public override void UseFunction()
        {
            if (GetGround(out var hit))
            {
                Instantiate(puddleObject, hit.point + spawnOffset, Quaternion.identity);
            }
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, Vector3.down * 100);
        }


    }
}