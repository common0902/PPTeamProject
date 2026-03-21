using _Script.Agent.FSM;
using UnityEngine;

namespace _Script.ScriptableObject
{
    [CreateAssetMenu(fileName = "new StateList SO", menuName = "FSM/State List", order = 0)]
    public class StateListSO : UnityEngine.ScriptableObject
    {
        public string enumName;
        public StateSO[] states;
    }
}