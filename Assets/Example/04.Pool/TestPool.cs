using UnityEngine;
using MiniFramework.Pool;
public class TestPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestPoolExample test1 = Pool<TestPoolExample>.Instance.Allocate();
        TestPoolExample test2 = Pool<TestPoolExample>.Instance.Allocate();
        Pool<TestPoolExample>.Instance.Recycle(test1);
        TestPoolExample test3 = Pool<TestPoolExample>.Instance.Allocate();

        Debug.Log("test1:" + test1.GetHashCode());
        Debug.Log("test2:" + test2.GetHashCode());
        Debug.Log("test3:" + test3.GetHashCode());
    }

}


public class TestPoolExample : IPoolable
{

    public bool IsRecycled { get; set; }

    public void OnRecycled()
    {
        Debug.Log(this.GetHashCode()+ ":被回收了");
    }

    public void OnAllocated()
    {
        Debug.Log(this.GetHashCode() + ":被分配了");
    }
}