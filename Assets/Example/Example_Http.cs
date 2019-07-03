using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Http : MonoBehaviour {

	// Use this for initialization
	void Start () {
		HttpRequest.Get(this,"www.baidu.com",(txt)=>Debug.Log(txt));
		
		string url = "http://b-ssl.duitang.com/uploads/item/201701/13/20170113161807_QzMt5.jpeg";
		HttpRequest.Download(this,url,Application.streamingAssetsPath,null);
	}
}
