using UnityEngine;
using MiniFramework;
public class Example_LoadPrefab : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
		if (GUI.Button(new Rect(1000,500,200,200),"生成Cube"))
		{
			ResManager.Instance.Load("Cube","prefab");
		}
    }
}
