using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class AutoDestroy : MonoBehaviour
    {
		public float Delay;
        // Use this for initialization
        void Start()
        {
			Destroy(gameObject,Delay);
        }
    }
}