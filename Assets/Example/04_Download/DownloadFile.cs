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
        yield return httpDownload.Download(Url, (result) =>
        {

        });
    }

    private void Update()
    {
        if (!httpDownload.isError)
            Debug.Log(httpDownload.Progress);
    }
}
