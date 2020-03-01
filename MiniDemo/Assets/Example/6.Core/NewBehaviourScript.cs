using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;
	public float radius;
	public float angle;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log(RangeCheck.FanCheck(transform,target.position,angle,radius));
    }
}
