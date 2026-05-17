using UnityEngine;

namespace HwanLib.Utility
{
    [DefaultExecutionOrder(-100)]
    public class LightSingleton<T> : MonoBehaviour where T : LightSingleton<T>
    {
        protected virtual void Awake()
        {
            T[] managers = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (managers.Length > 1)
                Destroy(gameObject);
            else
            {
                Initialize();
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}