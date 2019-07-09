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
        private string selectedAssetBundle;
        private Vector2 scorllPos;
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
            scorllPos = Vector2.zero;
        }
        private void OnGUI()
        {
            GUILayout.Label("打包平台");
            platform = (BuildTarget)EditorGUILayout.EnumPopup(platform);
            EditorPrefs.SetInt("platform", (int)platform);
            GUILayout.Label("压缩方式");
            option = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup(option);
            EditorPrefs.SetInt("option", (int)option);

            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (var item in assetBundleNames)
            {
                if (GUILayout.Toggle(selectedAssetBundle == item, item))
                {
                    if (selectedAssetBundle != item)
                    {
                        selectedAssetBundle = item;
                    }
                }
            }
            scorllPos = GUILayout.BeginScrollView(scorllPos);
            string[] prefabPaths = AssetDatabase.GetAssetPathsFromAssetBundle(selectedAssetBundle);
            long length = 0;
            foreach (var prefab in prefabPaths)
            {
                string[] dependencePaths = AssetDatabase.GetDependencies(prefab, false);
                foreach (var obj in dependencePaths)
                {
                    UnityEngine.Object target = AssetDatabase.LoadAssetAtPath(obj, typeof(UnityEngine.Object));
                    //length += Profiler.GetRuntimeMemorySizeLong(target);
                    Type type = typeof(Editor).Assembly.GetType("UnityEditor.TextureUtil");
                    MethodInfo methodInfo = type.GetMethod("GetStorageMemorySizeLong", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                    length += (long)methodInfo.Invoke(null, new object[] { target });
                    GUILayout.Label(obj);
                }
            }
            GUILayout.EndScrollView();
            GUILayout.Label("总计大小：" + UnitConvert.ByteAutoConvert(length));
            if (GUILayout.Button("Build"))
            {
                BuildPipeline.BuildAssetBundles(GetTargetPath(platform), option, platform);
            }
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