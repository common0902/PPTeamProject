using UnityEngine;

namespace _Works._JTH.Scripts.SO
{
    [CreateAssetMenu(fileName = "StageInfoSO", menuName = "Scriptable Objects/StageInfoSO")]
    public class StageInfoSO : ScriptableObject
    {
        public int titleIdx = 0;
        public int tutorialIdx = 1;
        public int stageStartIdx = 2;
        public int stageCount = 3;
    }
}
