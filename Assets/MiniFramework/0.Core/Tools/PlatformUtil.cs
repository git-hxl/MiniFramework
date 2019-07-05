
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

        public static bool IsIOS()
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
            if (IsIOS() || IsAndroid())
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

