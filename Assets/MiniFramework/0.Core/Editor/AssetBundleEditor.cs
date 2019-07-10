using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace MiniFramework
{
    public class AssetBundleEditor : EditorWindow
    {
        private BuildTarget platform;
        private BuildAssetBundleOptions option;
        private string version;
        [MenuItem("MiniFramework/AssetBundle")]
        public static void OpenWindow()
        {
            AssetBundleEditor window = (AssetBundleEditor)EditorWindow.GetWindow(typeof(AssetBundleEditor), false, "AssetBundle");
            window.Show();
        }
        private void Awake()
        {
            platform = (BuildTarget)EditorPrefs.GetInt("platform", 2);
            option = (BuildAssetBundleOptions)EditorPrefs.GetInt("option", 256);
            version = EditorPrefs.GetString("version", "1.0.0");
        }
        private void OnGUI()
        {
            GUILayout.Label("打包平台");
            platform = (BuildTarget)EditorGUILayout.EnumPopup(platform);

            GUILayout.Label("压缩方式");
            option = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup(option);

            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                GUILayout.Label(assetBundleNames[i]);
            }

            GUILayout.Label("版本信息");
            version = GUILayout.TextField(version);

            if (GUILayout.Button("打包"))
            {
                AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(GetTargetPath(platform), option, platform);
                Dictionary<string, string> config = new Dictionary<string, string>();
                config.Add("version", version);
                config.Add("platform",platform.ToString());
                SerializeUtil.ToJson(Application.streamingAssetsPath+ "/AssetBundle/Config.txt", config);
                FileUtil.Open(Application.streamingAssetsPath+"/AssetBundle");
            }
        }
        private void OnDestroy()
        {
            EditorPrefs.SetInt("platform", (int)platform);
            EditorPrefs.SetInt("option", (int)option);
            EditorPrefs.SetString("version", version);
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