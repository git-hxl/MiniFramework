using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class SerializeClass : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Test test = new Test();
        test.ID = 110;
        test.name = "xxoo";
        test.random = new double[] { 1 };

        string jsonData = JsonMapper.ToJson(test);
        
        Debug.Log(jsonData);
        Debug.Log(JsonMapper.ToObject(jsonData));
    }

}
public enum TestEnum
{
    A,
    B
}
public class Test
{
    public int ID;
    public string name;
    public double[] random;
}