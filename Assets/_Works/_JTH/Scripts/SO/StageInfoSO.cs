using UnityEngine;

namespace _Works._JTH.Scripts.SO
{
    [CreateAssetMenu(fileName = "StageInfoSO", menuName = "Scriptable Objects/StageInfoSO")]
    public class StageInfoSO : ScriptableObject
    {
        public int title = 0;
        public int tutorial = 1;
        public int stageStart = 2;
        public int stageEnd = 5;
    }
}
