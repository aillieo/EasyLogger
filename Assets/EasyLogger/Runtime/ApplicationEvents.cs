// -----------------------------------------------------------------------
// <copyright file="ApplicationEvents.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    internal static class ApplicationEvents
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
