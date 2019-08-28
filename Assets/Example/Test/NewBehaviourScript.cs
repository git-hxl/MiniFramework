using UnityEngine;
using MiniFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using System.Text;

public class NewBehaviourScript : MonoBehaviour
{
    public Texture2D Texture;
    public delegate void Delegate(string s);
    public Delegate TestDelegate;
    public event Delegate TestEvent;//只能在本类中被调用或=赋值，类外只可以+=、-=
    // Use this for initialization
    void Start()
    {
        //Texture = ResManager.Instance.Load("20171012051803_FHzS3") as Texture2D;

        //SceneManager.LoadScene("Caodi");

        // TestDelegate += Test;
        // TestDelegate("delegate");
        // TestDelegate.Invoke("delegate");

        // TestEvent += Test;
        // TestEvent.Invoke("event");
        // TestEvent("event");

        //Debug.Log(Marshal.SizeOf(new PackHead()));
 
    }
    void Test(byte[] data)
    {
        Debug.Log(Encoding.UTF8.GetString(data));
    }

    // Update is called once per frame
    void Update()
    {
        // if (Time.frameCount % 20 == 0)
        // {
        //     Debug.Log(Time.time);
        // }
        // if (Time.frameCount % 40 == 0)
        // {
        //     Debug.LogWarning(Time.time);
        // }
        // if (Time.frameCount % 60 == 0)
        // {
        //     Debug.LogError(Time.time);
        // }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 200, 200), "LoadScenes"))
        {
            StartCoroutine(AssetBundleLoader.LoadAssetBundle("/Users/apple/Desktop/scenes"));

        }
    }
}
