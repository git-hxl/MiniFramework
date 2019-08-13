using UnityEngine;
using MiniFramework;
public class InputTodo : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //2s后执行打印“hello”
        //this.Delay(2, () => { Debug.Log("请按下空格"); });
        //更加丰富的扩展 2s后 当检测到输入空格 执行打印“hello”
        this.Sequence()
        .Delay(2)
        .Until(() => Input.GetKeyDown(KeyCode.Space))
        .Event(() => Debug.Log("检测到空格输入"))
        .Execute();

        //暂停队列
        Sequence sequence1 = this.Repeat(1, -1, () => { Debug.Log(Time.time); });
        Sequence sequence2 = this.Sequence()
        .Event(() => Debug.Log("按下空格关闭队列"))
        .Until(() => Input.GetKeyDown(KeyCode.Space))
        .Event(() => { sequence1.Close(); Debug.Log("已关闭队列"); })
        .Execute();
    }

    private void Update()
    {

    }
}