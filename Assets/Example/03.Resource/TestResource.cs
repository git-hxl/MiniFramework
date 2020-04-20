using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Resource;
using MiniFramework.Config;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MiniFramework.WebRequest;

public class TestResource : MonoBehaviour
{
    public Image image;
    IDownloader downloader;
    public Slider slider;
    public Text text;

    float time;
    long lastSecondLength;
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.Instance.GetResourceRead.onReadCompleted += () =>
        {
            Debug.Log("----------------------------读取完成");
            image.sprite = ResourceManager.Instance.LoadAsset<Sprite>("star");
        };

        ResourceManager.Instance.GetResourceRead.onReadError += () =>
        {
            Debug.Log("----------------------------读取失败");
        };

        ResourceManager.Instance.GetResourceUpdate.onUpdateCompleted += () =>
        {
            Debug.Log("----------------------------更新完成");
        };

        ResourceManager.Instance.GetResourceUpdate.onUpdateError += () =>
        {
            Debug.Log("----------------------------更新失败");
        };


        //WebRequestManager.Instance.Download(
        //    "https://res06.bignox.com/full/20200413/69f2480a3da146cea3dcc19e461074f6.exe?filename=nox_setup_v6.6.0.5_full.exe", Application.streamingAssetsPath,
        //    out downloader);
    }



    private void Update()
    {
        slider.value = ResourceManager.Instance.GetResourceUpdate.GetDownloader.downloadProgress;

        text.text = ResourceManager.Instance.GetResourceUpdate.GetDownloader.downloadSpeed / 1024 + "mb/s";
    }
}
