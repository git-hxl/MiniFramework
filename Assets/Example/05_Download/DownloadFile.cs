using System.Collections;
using UnityEngine;
using MiniFramework;
public class DownloadFile : MonoBehaviour
{
    HttpDownload httpDownload;
    // Use this for initialization
    IEnumerator Start()
    {
        httpDownload = new HttpDownload(Application.streamingAssetsPath);
        yield return httpDownload.Download("http://b-ssl.duitang.com/uploads/item/201710/12/20171012051803_FHzS3.png");
    }

    // Update is called once per frame
    void Update()
    {
        if (httpDownload.Progress < 1)
            Debug.Log(httpDownload.Progress);
    }
}
