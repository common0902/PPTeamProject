using System;
using System.Collections.Generic;
using System.Linq;
//using _Script.Agent.Modules.StatSystem;
using _Script.SaveSystem;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Script.Agent.Modules
{
    public class OperatorSaveData : MonoBehaviour
    {

    }//, IModule, ISaveable //Sample Data. 
    //{
    //    [field:SerializeField] public int Exp { get; private set; }
    //    [field:SerializeField] public int Level { get; private set; }
    //    [field: SerializeField] public int EliteLevel { get; private set; }
        
    //    [field:SerializeField] public EventChannelSO ExpUpEventChannel { get; private set; }
    //    [field:SerializeField] public EventChannelSO SaveDataEventChannel { get; private set; }

    //    private Agent _agent;
    //    private IStatModule _statModule;
    //    public void Initialize(ModuleAgent moduleAgent)
    //    {
    //        _agent = moduleAgent as Agent;
    //        _statModule = moduleAgent.GetModule<IStatModule>();
    //    }

    //    [field:SerializeField] public SaveData SaveId { get; private set; }

    //    [Serializable]
    //    public struct StatData
    //    {
    //        public int statIndex;
    //        public float baseValue;
    //    }

    //    [Serializable]
    //    public struct PlayerOperatorSaveData
    //    {
    //        public int Exp;
    //        public int Level;
    //        public int EliteLevel;
    //        public List<StatData> StatList;
    //    }

    //    public string GetSaveData()
    //    {
    //        List<StatData> statDataList = _statModule.GetAllStat()
    //            .Select(stat => new StatData{statIndex = stat.Index, baseValue = stat.BaseValue})
    //            .ToList();

    //        PlayerOperatorSaveData saveData = new PlayerOperatorSaveData
    //        {
    //            Exp = Exp,
    //            Level = Level,
    //            EliteLevel = EliteLevel,
    //            StatList = statDataList
    //        };
            
    //        Debug.Log($"Get SaveData {saveData}");
    //        return JsonUtility.ToJson(saveData);
    //    }

    //    public void RestoreData(string data)
    //    {
    //        PlayerOperatorSaveData saveData = JsonUtility.FromJson<PlayerOperatorSaveData>(data);
    //        Level = saveData.Level;
    //        EliteLevel = saveData.EliteLevel;

    //        foreach (StatData statData in saveData.StatList)
    //        {
    //            //if (_statModule.TryGetStat(statData.statIndex, out StatSO stat))
    //            //{
    //            //    stat.BaseValue = statData.baseValue * (saveData.Level * (saveData.EliteLevel + 1) * 0.25f);
    //            //    Debug.Log($"Name : {stat.Name}, Value : {stat.BaseValue}, ReadValue : {stat.Value}");
    //            //}
    //        }// 60, 2
            
    //        Debug.Log($"Restore Save data {data}");
    //    }
    //}
}