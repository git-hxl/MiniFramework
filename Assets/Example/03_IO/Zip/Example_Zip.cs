using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Threading;

public class Example_Zip : MonoBehaviour
{
    bool isOk;
    // Use this for initialization
    IEnumerator Start()
    {
        string targetPath = Application.dataPath + "/StreamingAssets.zip";
        string savePath = Application.dataPath + "/StreamingAssets";
        Thread thread = new Thread(() =>
        {
            isOk = ZipUtil.UpZipFile(targetPath, savePath);
        });
        thread.Start();
		yield return new WaitUntil(()=>isOk);
		Debug.Log("解压完成");
    }
}
