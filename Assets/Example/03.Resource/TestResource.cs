using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Resource;
using MiniFramework.Config;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TestResource : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.Instance.onReadCompleted += () =>
        {
            Debug.Log("----------------------------读取完成");
            image.sprite = ResourceManager.Instance.LoadAsset<Sprite>("star");
        };

        ResourceManager.Instance.onReadError += () =>
        {
            Debug.Log("----------------------------读取失败");
        };

        ResourceManager.Instance.onUpdateCompleted += () =>
        {
            Debug.Log("----------------------------更新完成");
        };

        ResourceManager.Instance.onUpdateError += () =>
        {
            Debug.Log("----------------------------更新失败");
        };
    }
}
