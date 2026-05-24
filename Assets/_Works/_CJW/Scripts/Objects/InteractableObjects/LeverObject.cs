using System.Collections;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    public class LeverObject : AbstractInteractableObject
    {
        [SerializeField] private Transform leverTrm;
        [SerializeField] private Sabotage.Sabotage[] targetSabotages;
        [SerializeField] private float defaultRotation;
        [SerializeField] private float endRotation;
        [SerializeField] private float duration;

        protected override void Awake()
        {
            base.Awake();
            leverTrm.localRotation = Quaternion.Euler(defaultRotation, 0 , 0);
            if (targetSabotages.Length != 0)
            {
                foreach (Sabotage.Sabotage sabotage in targetSabotages)
                {
                    sabotage.LockSabotage();
                }
            }
        }

        [ContextMenu("Interact")]
        public override void HandleInteract()
        {
            base.HandleInteract();
            if (targetSabotages.Length != 0)
            {
                foreach (Sabotage.Sabotage sabotage in targetSabotages)
                {
                    // if(sabotage.IsLocked && !sabotage.IsUsed)
                        sabotage.UnlockSabotage();
                }
            }
            StartCoroutine(LabberCoroutine());
        }

        private IEnumerator LabberCoroutine()
        {
            float t = 0;

            Quaternion startRot = leverTrm.localRotation;
            Quaternion endRot = Quaternion.Euler(endRotation, 0, 0);

            while (t < duration)
            {
                t += Time.deltaTime;

                leverTrm.localRotation =
                    Quaternion.Lerp(startRot, endRot, t / duration);

                yield return null;
            }

            leverTrm.localRotation = endRot;
        }
    }
}