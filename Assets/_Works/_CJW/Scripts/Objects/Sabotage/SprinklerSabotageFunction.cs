using System;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage
{
    public class SprinklerSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private GameObject puddleObject;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector3 spawnOffset;
        public override void UseFunction()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundLayer))
            {
                Debug.Log("ASd");
                Instantiate(puddleObject, hit.point + spawnOffset, Quaternion.identity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, Vector3.down * 100);
        }
    }
}