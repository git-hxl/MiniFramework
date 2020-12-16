using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class PoolExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI() {

        if(GUILayout.Button("Allocate"))
        {
            Allocate();
        }

        if(GUILayout.Button("Recycle"))
        {
            Recycle();
        }

        if(GUILayout.Button("Clear"))
        {
            PoolManager.Instance.Clear();
        }
    }


    private void Allocate()
    {
        GameObject obj =  PoolManager.Instance.Allocate("Assets/MiniFramework 1/0.Example/Pool/Cube.prefab");
    }

    private void Recycle()
    {
        Poolable[] objs= GameObject.FindObjectsOfType<Poolable>();
        foreach (var item in objs)
        {
            if(!item.IsRecycled)
            {
                PoolManager.Instance.Recycle(item.gameObject);
                return;
            }
        }
    }
}
