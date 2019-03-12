using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Player!=null){
			transform.position = Player.position+new Vector3(0,10,-10);
			transform.LookAt(Player);
		}
	}
}
