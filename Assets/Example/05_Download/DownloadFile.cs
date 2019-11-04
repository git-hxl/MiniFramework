using System.Collections;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class DownloadFile : MonoBehaviour
{
    public string Url;
    public Texture2D Texture2D;
    HttpDownload httpDownload;
    // Use this for initialization
    IEnumerator Start()
    {
        httpDownload = new HttpDownload(Application.dataPath+"/Resources");
        yield return httpDownload.Download(Url, Callback);
    }
    private void Update()
    {
        if (!httpDownload.IsDone)
        {
            Debug.Log(httpDownload.Progress);
        }
    }
    void Callback(bool isSuccess)
    {
        if (isSuccess)
        {
            Texture2D = ResourceManager.Instance.Load<Texture2D>(httpDownload.FileName);
        }
    }
}
