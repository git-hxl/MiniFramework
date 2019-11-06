using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Threading;

public class Example_Zip : MonoBehaviour
{
    // Use this for initialization
    IEnumerator Start()
    {
        string targetPath = Application.dataPath + "/StreamingAssets.zip";
        string savePath = Application.dataPath + "/StreamingAssets";
        yield return ZipUtil.UpZipFile(targetPath, savePath, () =>
        {
            Debug.Log("解压完成");
        });
    }
}
