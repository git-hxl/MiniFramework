using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_ActionChain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//2s后执行打印“hello”
		this.Delay(2,()=>{Debug.Log("hello");});
		//更加丰富的扩展 2s后 当检测到输入空格 执行打印“hello”
		this.Sequence().Delay(2).Until(()=>Input.GetKeyDown(KeyCode.Space)).Event(()=>Debug.Log("space")).Execute();
		//可以直接声明Sequence对象(无缓存机制)
		Sequence sequence = new Sequence(this);
		sequence.Delay(2).Until(()=>Input.GetKeyDown(KeyCode.Space)).Event(()=>Debug.Log("space"));
		sequence.Execute();
	}

}
