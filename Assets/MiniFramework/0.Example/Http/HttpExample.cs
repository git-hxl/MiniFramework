using UnityEngine;
using MiniFramework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class HttpExample : MonoBehaviour
{
    public string testUrl;
    public string imgUrl;

    public Image sprite;
    Downloader downloader;

    float progress;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        downloader = new Downloader(Application.streamingAssetsPath, testUrl);
        //yield return downloader.Download();
        yield return HttpManager.Instance.Get(imgUrl, (handler) =>
        {
            sprite.sprite = handler;
            sprite.preserveAspect = true;
        });
    }

    private void OnGUI()
    {
        Debug.Log(downloader.progress);
        GUI.HorizontalSlider(new Rect(10, 10, 100, 20), downloader.progress, 0f, 1f);
    }
}
