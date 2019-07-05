using System.IO;
using UnityEditor;
using UnityEngine;

namespace MiniFramework
{
    public class AssetBundleEditor
    {
        [MenuItem("MiniFramework/AssetBundle/Build iOS")]
        public static void BuildABiOS()
        {
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.iOS), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
        }
        [MenuItem("MiniFramework/AssetBundle/Build Android")]
        public static void BuildABAndroid()
        {
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.Android), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }
        [MenuItem("MiniFramework/AssetBundle/Build Window")]
        public static void BuildABWindow()
        {        
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.StandaloneWindows), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        }
        [MenuItem("MiniFramework/AssetBundle/Build OSX")]
        public static void BuildABOSX()
        {        
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.StandaloneOSX), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
        }
        private static string GetTargetPath(BuildTarget platform)
        {
            string outputPath = Application.streamingAssetsPath + "/AssetBundle/" + platform;
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            return outputPath;
        }
    } 
}