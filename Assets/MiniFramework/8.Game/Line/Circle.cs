using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Circle : MonoBehaviour {

	private float t = 0.55228475f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawLine()
		for (int i = 0; i < 100; i++)
		{
			Vector3 pos1 = BezierUtil.Curve(i/100,new Vector3(0,1,0),new Vector3(t,1,0),new Vector3(1,t,0),new Vector3(1,0,0));
			Vector3 pos2 = BezierUtil.Curve((i+1)/100,new Vector3(0,1,0),new Vector3(t,1,0),new Vector3(1,t,0),new Vector3(1,0,0));
			Debug.DrawLine(pos1,pos2,Color.red);
		}

		Debug.DrawLine(Vector3.zero,new Vector3(10,0,0));

		for (int i = 0; i < 100; i++)
		{
			Vector3 pos1 = BezierUtil.Curve(i/100,new Vector3(-10,0,0),new Vector3(0,10,0),new Vector3(10,0,0));
			Vector3 pos2= BezierUtil.Curve((i+1)/100,new Vector3(-10,0,0),new Vector3(0,10,0),new Vector3(10,0,0));
			Debug.DrawLine(pos1,pos2,Color.yellow);
		}
	}
}
