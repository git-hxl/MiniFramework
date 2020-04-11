using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class SequenceExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.Sequence().Delay(2f).Event(()=>Debug.Log("2s")).Begin();

        this.Sequence().Delay(3f)
        .Event(()=>Debug.Log("3s 可以按下空格"))
        .Until(()=>Input.GetKeyDown(KeyCode.Space))
        .Event(()=>Debug.Log("检测到空格键"))
        .Begin();

        SequenceNode sequenceNode = this.Sequence().Repeat(-1,1,()=>Debug.Log(Time.time)).Begin();

        this.Sequence().Until(()=>Input.GetKeyDown(KeyCode.Escape)).Event(()=>sequenceNode.Stop()).Begin();
    }
}
