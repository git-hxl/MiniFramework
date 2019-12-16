using System.IO;
using UnityEditor;
using UnityEngine;
using LitJson;
namespace MiniFramework
{
    struct Config
    {
        public string version;
    }
    public class AssetBundleEditor : EditorWindow
    {
        private BuildTarget platform;
        private BuildAssetBundleOptions option;
        private string version;
        private string path;

        [MenuItem("MiniFramework/AssetBundle")]
        public static void OpenWindow()
        {
            AssetBundleEditor window = (AssetBundleEditor)EditorWindow.GetWindow(typeof(AssetBundleEditor), false, "AssetBundle");
            window.Show();
        }
        private void Awake()
        {
            platform = (BuildTarget)EditorPrefs.GetInt("Mini_Platform", 2);
            option = (BuildAssetBundleOptions)EditorPrefs.GetInt("Mini_Option", 256);
            version = EditorPrefs.GetString("Mini_Version", "1.0.0");
            path = EditorPrefs.GetString("Mini_Path", Application.streamingAssetsPath);
        }
        private void OnGUI()
        {
            GUILayout.Label("保存路径");
            GUILayout.BeginHorizontal();
            path = GUILayout.TextField(path);
            if (GUILayout.Button("选择文件夹"))
            {
                string selectPath = EditorUtility.OpenFolderPanel("资源保持路径", Application.dataPath, "");
                if (!string.IsNullOrEmpty(selectPath))
                {
                    path = selectPath;
                }
            }
            GUILayout.EndHorizontal();

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

            if (GUILayout.Button("生成config文件"))
            {
                CreateConfig();
            }
            if (GUILayout.Button("打开StreamingAssets目录"))
            {
                EditorUtility.RevealInFinder(Application.streamingAssetsPath);
            }
            if (GUILayout.Button("打开PersistentData目录"))
            {
                EditorUtility.RevealInFinder(Application.persistentDataPath);
            }
            if (GUILayout.Button("打包"))
            {
                BuildPipeline.BuildAssetBundles(GetTargetPath(platform), option, platform);
                CreateConfig();
            }
        }
        private void OnDestroy()
        {
            EditorPrefs.SetInt("Mini_Platform", (int)platform);
            EditorPrefs.SetInt("Mini_Option", (int)option);
            EditorPrefs.SetString("Mini_Version", version);
            EditorPrefs.SetString("Mini_Path", path);
        }
        private string GetTargetPath(BuildTarget platform)
        {
            string outputPath = path + "/" + platform;
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            return outputPath;
        }
        private void CreateConfig()
        {
            Config config = new Config();
            config.version = version;
            File.WriteAllText(GetTargetPath(platform) + "/config.txt", JsonMapper.ToJson(config));
            AssetDatabase.Refresh();
            Debug.Log("写入成功");
        }
    }
}