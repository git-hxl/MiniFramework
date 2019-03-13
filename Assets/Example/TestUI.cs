using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class TestUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UIManager.Instance.OpenUI("Art Number").GetComponent<ArtNumber>().Number = Time.frameCount.ToString();
	}
}
