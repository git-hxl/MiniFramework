using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Action : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DelayAction.Excute(this, 2, () => Debug.Log("2s执行"));
        UntilAction.Excute(this, () => Input.GetKeyDown(KeyCode.Space), () => Debug.Log("按下Space"));
        RepeatAction.Excute(this, 5, -1, () => Debug.Log("每隔5秒 重复无限次"));

        ActionChain chain = new ActionChain(this);
        chain.Delay(5, () => Debug.Log("hello"))
            .Until(() => Input.GetKeyDown(KeyCode.Space), () => Debug.Log("Space"))
            .Repeat(2, 2, () => Debug.Log("每隔2秒 重复2次"))
            .Excute();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
