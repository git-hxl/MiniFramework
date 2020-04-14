using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using MiniFramework.Pool;
public class AutoRecycleObject : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        this.Sequence().Delay(1).Event(() =>
        {
            ObjectPool.Instance.Recycle(gameObject);
        }).Begin();
    }
  
}
