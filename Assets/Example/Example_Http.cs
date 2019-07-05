﻿using System.Collections;
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
        download = new HttpDownload(this, url, Application.streamingAssetsPath);
        download.Download();
    }

    private void Update()
    {
        Debug.Log("进度:"+download.GetProgress+" 当前："+download.GetCurLength+"字节 总计："+download.GetTotalLength+"字节");
    }
}
