using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace MiniFramework
{
    public class ZipEditor : EditorWindow
    {
        [MenuItem("Assets/压缩")]
        static void Zip()
        {
            if (Selection.assetGUIDs == null) return;
            string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            string zipName = assetPath.Substring(assetPath.LastIndexOf('/') + 1) + ".zip";
            ZipUtil.ZipDirectory(assetPath, Application.dataPath, zipName);
            AssetDatabase.Refresh();
            Debug.Log("压缩完成");
        }
    }
}