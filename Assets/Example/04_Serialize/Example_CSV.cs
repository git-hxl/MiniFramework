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

		foreach (var item in cSVData)
		{
			Debug.Log(item.ToString());
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
