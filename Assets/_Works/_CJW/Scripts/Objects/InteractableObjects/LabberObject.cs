using System.Collections;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    public class LabberObject : AbstractInteractableObject
    {
        [SerializeField] private Transform labberTrm;
        [SerializeField] private float defaultRotation;
        [SerializeField] private float endRotation;
        [SerializeField] private float duration;

        private void Awake()
        {
            labberTrm.localRotation = Quaternion.Euler(defaultRotation, 0 , 0);
        }

        public override void HandleInteract()
        {
            base.HandleInteract();
            StartCoroutine(LabberCoroutine());
        }

        private IEnumerator LabberCoroutine()
        {
            float t = 0;
            while (t < duration)
            {
                Debug.Log(t);
                t += Time.deltaTime;
                labberTrm.localRotation = 
                    Quaternion.Lerp(labberTrm.rotation, Quaternion.Euler(endRotation, 0, 0), t / duration);
                yield return null;
            }

        }
    }
}