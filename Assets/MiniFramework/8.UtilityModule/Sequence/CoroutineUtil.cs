using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class CoroutineUtil : MonoBehaviour
    {
        private static CoroutineUtil instance = null;
        public static CoroutineUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("CoroutineUtil");
                    instance = obj.AddComponent<CoroutineUtil>();
                }
                return instance;
            }
        }

        public CoroutineNode Create()
        {
            return new CoroutineNode(this);
        }
        
        private void OnDestroy()
        {
            instance = null;
        }
    }
}