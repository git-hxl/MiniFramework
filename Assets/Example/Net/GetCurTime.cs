using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System;
public class GetCurTime : MonoBehaviour {
	ArtNumber artNumber;
	// Use this for initialization
	void Start () {
		artNumber = GetComponent<ArtNumber>();
	}
	
	// Update is called once per frame
	void Update () {
		artNumber.Number = DateTime.Now.ToString();
	}
}
