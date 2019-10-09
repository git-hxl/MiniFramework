using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Action : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        RepeatAction.Excute(this, 1, -1, () => Debug.Log(Time.time));

        ActionChain chain = new ActionChain(this);
        chain.Until(() => Input.GetKeyDown(KeyCode.Space), () => {Debug.Log("按下空格键");})
        .Delay(5, () => Debug.Log("5s"))
        .Excute();

		UntilAction.Excute(this,()=>Input.GetKeyDown(KeyCode.E),()=>{chain.Stop();Debug.Log("执行中断");});
    }

    // Update is called once per frame
    void Update()
    {

    }
}
