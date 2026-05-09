using UnityEngine;

namespace _Works._CJW.Scripts.Objects.InteractableObjects
{
    [CreateAssetMenu(fileName = "Interactable Object data", menuName = "", order = 0)]
    public class InteractableObjectDataSo : ScriptableObject
    {
        [field: SerializeField] public string SabotageName { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string SabotageDesc { get; private set; } 
    }
}