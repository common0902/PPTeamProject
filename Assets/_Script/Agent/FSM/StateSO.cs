using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.FSM
{
    [CreateAssetMenu(fileName = "new StateSO", menuName = "FSM/StateSO", order = 15)]
    public class StateSO : UnityEngine.ScriptableObject
    {
        public string stateName;
        public string className;
        public int stateIndex;
        public AnimationHashSO stateParam;
    }
}