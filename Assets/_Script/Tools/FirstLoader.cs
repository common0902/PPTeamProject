using System;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Script.Tools
{
    [DefaultExecutionOrder(-10)]
    public class FirstLoader : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO SaveDataEventChannelSO { get; private set; }
        
        
        //Awake에서는 구독이 일어난다 Awake에서는 구독이 일어난다 Awake에서는 구독이 일어난다 Awake에서는 구독이 일어난다 Awake에서는 구독이 일어난다 
        private void Start()
        {
            SaveDataEventChannelSO.RaiseEvent(DataEvents.DataLoadEvent);
        }
    }
}