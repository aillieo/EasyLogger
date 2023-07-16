using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AillieoUtils.EasyLogger
{
    public static class ApplicationEvents
    {
        public static event Action onApplicationLaunch;

        public static event Action onApplicationQuit;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void OnApplicationLaunch()
        {
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    onApplicationQuit?.Invoke();
                }
            };

            onApplicationLaunch?.Invoke();
        }
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnApplicationLaunch()
        {
            Application.quitting += () => onApplicationQuit?.Invoke();

            onApplicationLaunch?.Invoke();
        }
#endif
    }
}
