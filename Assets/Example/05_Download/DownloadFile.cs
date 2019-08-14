using System.Collections;
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
        httpDownload = new HttpDownload(Application.streamingAssetsPath);
        yield return httpDownload.Download(Url);
    }

    // Update is called once per frame
    void Update()
    {
        if (httpDownload.Progress < 1)
            Debug.Log(httpDownload.Progress);
    }
}
