using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Http : MonoBehaviour
{
    HttpDownload download;
    // Use this for initialization
    void Start()
    {
        string url = "http://b-ssl.duitang.com/uploads/item/201701/13/20170113161807_QzMt5.jpeg";
        List<string> urls = new List<string>();
        urls.Add(url);
        urls.Add(url);
        download = new HttpDownload(this, urls, Application.streamingAssetsPath);
    }

    private void Update()
    {
        if (!download.IsCompleted)
            Debug.Log("进度:" + download.GetProgress + " 已完成:" + download.CompletedTask + " 当前：" + download.GetCurLength + "字节 总计：" + download.GetTotalLength + "字节");
    }
}
