using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Pool;
public class TestObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           GameObject obj = ObjectPool.Instance.Allocate("Assets/Example/04.Pool/PoolObject/Cube.prefab");

           obj.transform.position = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
        }

    }
}
