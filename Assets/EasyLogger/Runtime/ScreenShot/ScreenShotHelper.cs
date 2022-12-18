using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

namespace AillieoUtils.EasyLogger
{
    public static class ScreenShotHelper
    {
        private static readonly string extensionJpg = ".jpg";

        public static Task<string> CaptureAndSave(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                string filename = $"{Application.productName}-{DateTime.Now:yyyyMMddHHmmssfff}{extensionJpg}";
                path = Path.Combine(GetDefaultDirectory(), filename);
            }
            else
            {
                if (string.IsNullOrEmpty(Path.GetDirectoryName(path)))
                {
                    path = Path.Combine(GetDefaultDirectory(), path);
                }

                if (Path.GetExtension(path) != extensionJpg)
                {
                    path = path + extensionJpg;
                }
            }

            return ScreenShotImpl.Instance.CaptureAndSave(path);
        }

        private static string GetDefaultDirectory()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "..", "ScreenShots");
#else
            return Path.Combine(Application.persistentDataPath, "ScreenShots");
#endif
        }
    }
}
