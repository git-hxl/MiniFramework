using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class FanCheck : MonoBehaviour
    {
		public float angle=30;
		public float  radius=2;
		public Transform target;

        // Update is called once per frame
        void Update()
        {
			Debug.Log(RangeCheck.FanCheck(transform,target.position,angle,radius));
        }
    }

}