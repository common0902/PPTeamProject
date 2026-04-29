using System;
using System.Collections.Generic;
using System.Linq;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Script.SaveSystem
{
    public class DataManager : MonoBehaviour
    {
        [Serializable]
        public struct SaveData //세이브 할 데이터들이다. Json으로 저장하기 떄문에 Data = string. Id 기반으로 특정 데이터를 불러온다.
        {
            public int Id;
            public string Data;
        }

        [Serializable]
        public struct DataCollection // 데이터 리스트가 필요할 때 잠깐 할당받기 위한 용도로 사용됨.
        {
            public List<SaveData> Collection;
        }

        [SerializeField] private string prefKey = "saveData"; // Data가 저장되어있는 prefs Key이다.
        
        private List<SaveData> _unUsedData = new List<SaveData>(); // 사용하지 않는 데이터들을 담는 List이다.
        [field: SerializeField] public EventChannelSO DataSaveEventChannel { get; private set; }

        private void Awake()
        {
            DataSaveEventChannel.AddListener<StoreDataEvent>(HandleStorePrefEvent);
            DataSaveEventChannel.AddListener<RestoreDataEvent>(HandleRestorePrefEvent);
        }


        private void OnDestroy()
        {
            DataSaveEventChannel.RemoveListener<StoreDataEvent>(HandleStorePrefEvent);
            DataSaveEventChannel.RemoveListener<RestoreDataEvent>(HandleRestorePrefEvent);
        }

        private void HandleStorePrefEvent(StoreDataEvent @event)
        {
            string saveData = GetSceneSaveData();       //Save Data : 씬 전체를 스캔떠서 IStorable요소를 string으로 저장한다.
            PlayerPrefs.SetString(prefKey, saveData);
            Debug.Log($"Data Save!! {saveData}");
        }
        private void HandleRestorePrefEvent(RestoreDataEvent evt)
        {
            string loadJson = PlayerPrefs.GetString(prefKey, string.Empty);
            RestoreData(loadJson);
        }

        private string GetSceneSaveData()
        {
            IEnumerable<IStorable> saveableObjects =
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IStorable>();
            
            List<SaveData> toSaveData = new List<SaveData>();
            foreach (IStorable saveable in saveableObjects)
            {
                toSaveData.Add(new SaveData { Id = saveable.SaveId.Id, Data = saveable.StoreData() });
            }

            toSaveData.AddRange(_unUsedData);

            DataCollection dataCollection = new DataCollection { Collection = toSaveData };
            Debug.Log("Data : " + dataCollection);
            return JsonUtility.ToJson(dataCollection);
        }

        private void RestoreData(string json)
        {
            IEnumerable<IRestorable> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IRestorable>();
            DataCollection parsedData = string.IsNullOrEmpty(json) ? new DataCollection() : JsonUtility.FromJson<DataCollection>(json);
            //json값이 비지 않았다면 (저장된 값이 이미 있다면) json값을 들고오고, 저장한다. 비어있다면 새로 판다.

            _unUsedData.Clear();

            if (parsedData.Collection != null)
            {
                foreach (SaveData saveData in parsedData.Collection)
                {
                    IRestorable saveable = saveables.FirstOrDefault(s => s.SaveId.Id == saveData.Id);
                    if (saveable != null)
                    {
                        saveable.RestoreData(saveData.Data);
                    }
                    else
                    {
                        _unUsedData.Add(saveData);
                    }
                }
            }
            Debug.Log("Restore : " + parsedData);
        }
        
        [ContextMenu("Clear Pref Data")]
        public void ClearPref() => PlayerPrefs.DeleteKey(prefKey);
    }
}