using UnityEngine;
using UnityEditor;
using System.IO;
namespace MiniFramework
{
    public class ZipEditor
    {
        [MenuItem("Assets/压缩文件夹")]
        static void Zip()
        {
            if (Selection.assetGUIDs == null) return;
            string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            if (Directory.Exists(assetPath))
            {
                string zipName = assetPath.Substring(assetPath.LastIndexOf('/') + 1) + ".zip";
                string savePath = assetPath.Substring(0, assetPath.LastIndexOf('/'));
                ZipUtil.ZipDirectory(assetPath, savePath, zipName);
                AssetDatabase.Refresh();
                Debug.Log("压缩完成");
            }
            else
            {
                Debug.Log("请选择文件夹进行压缩");
            }
        }

        [MenuItem("Assets/解压压缩包")]
        static void UnZip()
        {
            if (Selection.assetGUIDs == null) return;
            string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            if (File.Exists(assetPath))
            {
                string savePath = assetPath.Substring(0, assetPath.LastIndexOf('.'));
                ZipUtil.UpZipFile(assetPath, savePath);
                AssetDatabase.Refresh();
                Debug.Log("解压完成");
            }
            else
            {
                Debug.Log("请选择文件进行压缩");
            }
        }
    }
}