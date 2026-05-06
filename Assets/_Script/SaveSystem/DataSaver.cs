using System.Collections;
using _Script.ScriptableObject.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            SceneManager.sceneUnloaded += StoreData;
            
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
                StoreData();
            }
        }

        private void StoreData(Scene _)
            => StoreData();
        
        private void StoreData()
            => saveChannel.RaiseEvent(SaveEvents.StoreDataEvent);
    }
}
