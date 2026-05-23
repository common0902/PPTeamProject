using System.Collections;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    public class LabberObject : AbstractInteractableObject
    {
        [SerializeField] private Transform labberTrm;
        [SerializeField] private Sabotage.Sabotage[] targetSabotages;
        [SerializeField] private float defaultRotation;
        [SerializeField] private float endRotation;
        [SerializeField] private float duration;

        private void Awake()
        {
            labberTrm.localRotation = Quaternion.Euler(defaultRotation, 0 , 0);
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
                    sabotage.UnlockSabotage();
                }
            }
            StartCoroutine(LabberCoroutine());
        }

        private IEnumerator LabberCoroutine()
        {
            float t = 0;

            Quaternion startRot = labberTrm.localRotation;
            Quaternion endRot = Quaternion.Euler(endRotation, 0, 0);

            while (t < duration)
            {
                t += Time.deltaTime;

                labberTrm.localRotation =
                    Quaternion.Lerp(startRot, endRot, t / duration);

                yield return null;
            }

            labberTrm.localRotation = endRot;
        }
    }
}