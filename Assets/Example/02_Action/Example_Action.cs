using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Action : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DelayAction.Excute(this, 2, () => Debug.Log(222222));
        UntilAction.Excute(this, () => Input.GetKeyDown(KeyCode.Space), () => Debug.Log("Space"));
        RepeatAction.Excute(this, 1, -1, () => Debug.Log("repeat 1"));

        ActionChain chain = new ActionChain(this);
        chain.Delay(5, () => Debug.Log(555555))
            .Until(() => Input.GetKeyDown(KeyCode.W), () => Debug.Log("W"))
            .Repeat(2, 2, () => Debug.Log("repeat 2"))
            .Excute();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
