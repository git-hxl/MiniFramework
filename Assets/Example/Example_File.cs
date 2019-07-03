﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_File : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FileUtil.WriteFile(Application.streamingAssetsPath+"/Test.txt","hello--");
		FileUtil.WriteFile(Application.streamingAssetsPath+"/Test.txt","world",true);

		string content = FileUtil.ReadFile(Application.streamingAssetsPath+"/Test.txt");
		Debug.Log(content);

		Debug.Log(FileUtil.ISExitDir(Application.streamingAssetsPath));
	}

}