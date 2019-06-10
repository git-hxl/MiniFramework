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
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.iOS), BuildAssetBundleOptions.None, BuildTarget.iOS);
        }
        [MenuItem("MiniFramework/AssetBundle/Build Android")]
        public static void BuildABAndroid()
        {
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.Android), BuildAssetBundleOptions.None, BuildTarget.Android);
        }
        [MenuItem("MiniFramework/AssetBundle/Build Window")]
        public static void BuildABWindow()
        {        
            BuildPipeline.BuildAssetBundles(GetTargetPath(BuildTarget.StandaloneWindows), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
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