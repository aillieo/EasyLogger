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
        private static void OnEditorLaunch()
        {
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    OnEditorApplicationQuit();
                }
            };

            onApplicationLaunch?.Invoke();
        }
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnSessionLaunch()
        {
            Application.quitting += OnApplicationQuit;

            onApplicationLaunch?.Invoke();
        }
#endif

#if UNITY_EDITOR
        private static void OnEditorApplicationQuit()
        {
            onApplicationQuit?.Invoke();
        }
#else
        private static void OnApplicationQuit()
        {
            onApplicationQuit?.Invoke();
        }
#endif
    }
}
