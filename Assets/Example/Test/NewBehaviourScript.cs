using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class NewBehaviourScript : MonoBehaviour
{
    public Texture2D Texture;
    // Use this for initialization
    void Start()
    {
        Texture = ResManager.Instance.Load("20171012051803_FHzS3") as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.captureFramerate%20 ==0){
        	Debug.Log(Time.time);
        }
        if(Time.captureFramerate%40 ==0){
        	Debug.LogWarning(Time.time);
        }
        if(Time.captureFramerate%60 ==0){
        	Debug.LogError(Time.time);
        }
    }
}
