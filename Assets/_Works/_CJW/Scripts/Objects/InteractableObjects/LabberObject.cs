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

        // [Conte]
        
        private IEnumerator LabberCoroutine()
        {
            float t = 0;
            while (t < duration)
            {
                t += Time.deltaTime;
                transform.rotation = 
                    Quaternion.Lerp(transform.rotation, Quaternion.Euler(endRotation, 0, 0), t);
            }

            yield return null;
        }
    }
}