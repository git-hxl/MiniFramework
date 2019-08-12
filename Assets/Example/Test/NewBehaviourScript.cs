using UnityEngine;
using MiniFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public Texture2D Texture;
    // Use this for initialization
    void Start()
    {
        Texture = ResManager.Instance.Load("20171012051803_FHzS3") as Texture2D;

        //SceneManager.LoadScene("Caodi");
    }

    // Update is called once per frame
    void Update()
    {
        // if(Time.captureFramerate%20 ==0){
        // 	Debug.Log(Time.time);
        // }
        // if(Time.captureFramerate%40 ==0){
        // 	Debug.LogWarning(Time.time);
        // }
        // if(Time.captureFramerate%60 ==0){
        // 	Debug.LogError(Time.time);
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
