using System.IO;
using UnityEditor;
using UnityEngine;
using MiniFramework;
using System.Collections.Generic;

public class MiniExtension
{
    [MenuItem("Assets/获取AssetPath")]
    static void GetResAssetPath()
    {
        if (Selection.assetGUIDs == null) return;
        string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        TextEditor textEditor = new TextEditor();
        textEditor.text = assetPath;
        textEditor.OnFocus();
        textEditor.Copy();
    }
    [MenuItem("MiniFramework/Tool/打开StreamingAssets")]
    static void OpenStreamingAssetsFinder()
    {
        EditorUtility.RevealInFinder(Application.streamingAssetsPath);
    }
    [MenuItem("MiniFramework/Tool/打开PersisentData")]
    static void OpenPersistentDataFinder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    
    [MenuItem("MiniFramework/Tool/Clear PlayerPrefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    [MenuItem("Assets/生成贴花配置")]
    static void GenZipConfig()
    {
        if (Selection.assetGUIDs == null) return;
        string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        if (Directory.Exists(assetPath))
        {
            if (File.Exists(assetPath + "/config.txt"))
            {
                File.Delete(assetPath + "/config.txt");
            }
            string[] files = Directory.GetFiles(assetPath);
            List<string> fileConfig = new List<string>();
            foreach (var item in files)
            {
                if (item.Contains(".meta"))
                {
                    continue;
                }
                string fileName = item.Substring(item.LastIndexOf('/') + 1);
                string hash = MiniFramework.FileUtil.GetMD5(item);
                fileConfig.Add(fileName + ":" + hash);
            }
            //fileConfig.re
            File.WriteAllLines(assetPath + "/config.txt", fileConfig);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("Assets/打印文件Hash")]
    static void DebugFileHash()
    {
        if (Selection.assetGUIDs == null) return;
        string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        string hash = MiniFramework.FileUtil.GetMD5(assetPath);
        Debug.Log(hash);
    }
}