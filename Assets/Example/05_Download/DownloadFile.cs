using System.Collections;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class DownloadFile : MonoBehaviour
{
    public string Url;
    HttpDownload httpDownload;
    // Use this for initialization
    IEnumerator Start()
    {
        httpDownload = new HttpDownload(Application.dataPath);
        yield return httpDownload.Download(Url);
    }

    private void Update() {
        Debug.Log(httpDownload.Progress);
    }
}
