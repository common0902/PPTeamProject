using System;
using DG.Tweening;
using GameLib.SoundSystem;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    public class SprinklerSabotageFunction : AbstractSabotageFunctionModule
    {
        [SerializeField] private AbstractObject puddleObject;
        [SerializeField] private float duration;
        [SerializeField] private Vector3 detectSize;
        public override void UseFunction()
        {
            base.UseFunction(); 

            var puddle = Instantiate(puddleObject, new Vector3(transform.position.x,1, transform.position.z), Quaternion.identity);
            puddle.SetLifetime(duration);
            puddle.InitSize(detectSize);
            }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(new Vector3(transform.position.x,1, transform.position.z), detectSize);
        }
    }
}