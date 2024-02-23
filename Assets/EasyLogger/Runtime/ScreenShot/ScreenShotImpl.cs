// -----------------------------------------------------------------------
// <copyright file="ScreenShotImpl.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;

    internal class ScreenShotImpl : SingletonMonoBehaviour<ScreenShotImpl>
    {
        public Task<string> CaptureAndSave(string path)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            this.StartCoroutine(this.InternalCaptureAndSave(path, tcs));
            return tcs.Task;
        }

        private IEnumerator InternalCaptureAndSave(string path, TaskCompletionSource<string> tcs)
        {
            yield return new WaitForEndOfFrame();

            string result = null;

            try
            {
                Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

                screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
                screenShot.Apply();

                byte[] bytes = screenShot.EncodeToJPG();

                path = FileUtils.MakeUniqueNumberSuffix(path);
                FileUtils.EnsureDirectory(Path.GetDirectoryName(path));

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Flush();
                    result = path;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                tcs.SetResult(result);
            }
        }
    }
}
