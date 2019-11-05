using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_CSV : MonoBehaviour
{
	public TextAsset Csv;
    // Use this for initialization
    void Start()
    {
        CSVData cSVData = CSVUtil.FromCSV(Csv.text);
        //支持遍历输出
		// foreach (var item in cSVData)
		// {
		// 	Debug.Log(item);
		// }
        //也可以直接打印
        Debug.Log(cSVData);

        //输出第2行标题为[奖励金币数值]的值
        //Debug.Log(cSVData[2]["奖励金币数值"]);


         //三种写法结果相同;目前只支持string格式 下标从0开始
         Debug.Log(cSVData[3]);
         Debug.Log(cSVData[3].ToString());
         Debug.Log((string)cSVData[3]);

         Debug.Log(cSVData[3]["解锁武器倍数"]);
         Debug.Log(cSVData[3]["解锁武器倍数"].ToString());
         Debug.Log((string)cSVData[3]["解锁武器倍数"]);
    }
}
