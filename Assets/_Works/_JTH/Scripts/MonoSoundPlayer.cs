using _Script.ScriptableObject.Event;
using _Works._JTH.Scripts.SO;
using GameLib.SoundSystem;
using HwanLib.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Works._JTH.Scripts
{
    public class MonoSoundPlayer : LightSingleton<MonoSoundPlayer>
    {
        [SerializeField] private StageInfoSO stageInfoSO;
        [SerializeField] private EventChannelSO soundChannel;
        [SerializeField] private SoundClipSO clipSO;

        protected override void Initialize()
        {
            base.Initialize();
            
            SceneManager.sceneLoaded += SceneLoadHandler;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneLoadHandler;
        }

        private void SceneLoadHandler(Scene scene, LoadSceneMode arg1)
        {
            int buildIdx = scene.buildIndex;
            if (buildIdx >= stageInfoSO.tutorialIdx
                && buildIdx <= stageInfoSO.stageStartIdx + stageInfoSO.stageCount - 1)
            {
                soundChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(Vector3.zero, clipSO));
            }
        }
    }
}
