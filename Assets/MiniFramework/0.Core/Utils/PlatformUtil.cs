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
    }
}

