using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(Player!=null){
			Vector3 followPos = Player.position+new Vector3(0,10,-10);
			transform.position =Vector3.MoveTowards(transform.position,followPos,Time.deltaTime*10);
			transform.LookAt(Player);
		}
	}
}
