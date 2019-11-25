using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_GMsgSend : MonoBehaviour
{

    void Start()
    {
        GameMsgManager.Instance.Dispatch<string>(GameMsgID.Test, "hello");
        GameMsgManager.Instance.Dispatch<int>(GameMsgID.Test, 123);
    }
}