using UnityEngine;
namespace MiniFramework
{
    public static class PlatformUtil
    {
        public static bool IsEditor()
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
        /// <summary>
        /// Mac OS X, Windows or Linux
        /// </summary>
        /// <returns></returns>
        public static bool IsPC()
        {
#if UNITY_STANDALONE
            return true;
#else
            return false;
#endif
        }
        public static bool IsWindows()
        {
#if UNITY_EDITOR_WIN||UNITY_STANDALONE_WIN
            return true;
#else
            return false;
#endif
        }
        public static bool IsMac()
        {
#if UNITY_EDITOR_OSX||UNITY_STANDALONE_OSX
            return true;
#else
            return false;
#endif
        }

        public static bool IsiOS()
        {
#if UNITY_IOS
            return true;
#else
            return false;
#endif
        }
        public static bool IsAndroid()
        {
#if UNITY_ANDROID
            return true;
#else
            return false;
#endif
        }
        /// <summary>
        /// 安卓或者iOS
        /// </summary>
        /// <returns></returns>
        public static bool IsPhone()
        {
            if (IsiOS() || IsAndroid())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetPlatformName()
        {
            string platform = "";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    platform = "StandaloneWindows"; break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    platform = "StandaloneOSX"; break;
                case RuntimePlatform.Android:
                    platform = "Android"; break;
                case RuntimePlatform.IPhonePlayer:
                    platform = "iOS"; break;
            }
            return platform;
        }
    }
}

