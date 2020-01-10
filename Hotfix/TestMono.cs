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
            Debug.Log("DLL Awake:"+gameObject.name);
        }

        void OnEnable()
        {
            Debug.Log("DLL OnEnable:"+gameObject.name);

            StartCoroutine(TestCor());
        }
        void Start()
        {
            Debug.Log("DLL Start:"+gameObject .name);
        }

        //void Update()
        //{
        //    //Debug.Log("DLL "+gameObject.name);
        //}

        void OnDisable()
        {
            Debug.Log("DLL OnDisable:"+gameObject.name);
        }

        void OnDestroy()
        {
            Debug.Log("DLL OnDestroy:"+gameObject.name);
        }

        public IEnumerator TestCor()
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("5s过去了");
        }
    }

}
