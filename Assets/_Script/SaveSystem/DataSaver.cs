using System.Collections;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Script.SaveSystem
{
    [DefaultExecutionOrder(FirstSaveExecutionOrder)]
    public class DataSaver : MonoBehaviour
    {
        private const int FirstSaveExecutionOrder = -1_000_000_000;
        
        [SerializeField] private float autoSaveInterval;
        [SerializeField] private EventChannelSO saveChannel;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            StartCoroutine(RepeatSave());
        }

        private void Start()
        {
            saveChannel.RaiseEvent(SaveEvents.RestoreDataEvent);
        }

        private IEnumerator RepeatSave()
        {
            WaitForSeconds wait = new WaitForSeconds(autoSaveInterval);
            while (true)
            {
                yield return wait;
                saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
            }
        }
    }
}
