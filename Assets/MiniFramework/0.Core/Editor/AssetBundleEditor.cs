using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
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

            GUILayout.BeginVertical("box");
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                GUILayout.Label(assetBundleNames[i]);
            }
            GUILayout.EndVertical();

            GUILayout.Label("版本信息");
            version = GUILayout.TextField(version);

            if (GUILayout.Button("打包"))
            {
                BuildPipeline.BuildAssetBundles(GetTargetPath(platform), option, platform);
                CreateConfig();
                FileUtil.Open(Application.streamingAssetsPath);
            }
            if (GUILayout.Button("打开StreamingAssets目录"))
            {
                FileUtil.Open(Application.streamingAssetsPath);
            }
            if (GUILayout.Button("打开PersistentData目录"))
            {
                FileUtil.Open(Application.persistentDataPath);
            }
            if (GUILayout.Button("生成config文件"))
            {
                CreateConfig();
            }
        }
        private void OnDestroy()
        {
            EditorPrefs.SetInt("platform", (int)platform);
            EditorPrefs.SetInt("option", (int)option);
            EditorPrefs.SetString("version", version);
        }
        private string GetTargetPath(BuildTarget platform)
        {
            string outputPath = Application.streamingAssetsPath + "/" + platform;
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            return outputPath;
        }
        private void CreateConfig()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("version", version);
            config.Add("platform", platform.ToString());
            SerializeUtil.ToJson(Application.streamingAssetsPath + "/config.txt", config);
        }
    }
}