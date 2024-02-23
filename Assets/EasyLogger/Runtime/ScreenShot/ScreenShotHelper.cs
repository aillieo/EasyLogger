// -----------------------------------------------------------------------
// <copyright file="ScreenShotHelper.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;

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
            return FileUtils.GetPersistentPath("ScreenShots");
        }
    }
}
