using UnityEngine;
using MiniFramework;
public class InputTodo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DelayEvent.Excute(this, 2, () => { Debug.Log(Time.time); });
        UntilEvent.Excute(this,()=>Input.GetKeyDown(KeyCode.Space),()=>Debug.Log("Space"));
        RepeatEvent.Excute(this,3,-1,()=>Debug.Log(Time.time));
    }

    private void Update()
    {

    }
}