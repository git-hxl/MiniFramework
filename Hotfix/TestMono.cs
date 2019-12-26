using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Hotfix
{
    public class TestMono : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("DLL Awake");
        }

        void OnEnable()
        {
            Debug.Log("DLL OnEnable");
        }
        void Start()
        {
            Debug.Log("DLL Start");
        }

        //void Update()
        //{
        //    //Debug.Log("DLL "+gameObject.name);
        //}

        void OnDisable()
        {
            Debug.Log("DLL OnDisable");
        }

        void OnDestroy()
        {
            Debug.Log("DLL OnDestroy");
        }
    }

}
