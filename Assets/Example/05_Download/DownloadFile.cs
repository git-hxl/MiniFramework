﻿using System.Collections;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class DownloadFile : MonoBehaviour
{
    public string Url = "http://img95.699pic.com/audio/967/677/5aefb81824683_all.mp3";
    HttpDownload httpDownload;
    // Use this for initialization
    IEnumerator Start()
    {
        httpDownload = new HttpDownload(Application.dataPath);
        yield return httpDownload.Download(Url, Callback);
    }

    void Callback(bool isSuccess)
    {
        if (isSuccess)
        {
            AudioClip ac = ResourceManager.Instance.Load<AudioClip>("5aefb81824683_all.mp3");
            AudioManager.Instance.PlayMusic(ac);
        }
    }
}
