using System.Collections;
using System.Collections.Generic;
using MiniFramework;
using UnityEngine;

public class Recv1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.Regist(111, eventHandler1);
    }


    void eventHandler1(object param)
    {
        Debug.Log(gameObject.name + ": recived:" + param.ToString());
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(111, eventHandler1);
    }

}
