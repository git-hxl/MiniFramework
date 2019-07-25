using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using ProtoBuf;
public class Example_Serialize : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        TestJson json = new TestJson();
        json.ID = 110;
        json.name = "xxoo";
        json.random.Add(1.0f);
        json.random.Add(2.0f);
        
        string jsonData = SerializeUtil.ToJson(json);
        Debug.Log(jsonData);

        TestProto proto = new TestProto();
        proto.ID = 110;
        proto.name = "xxoo";
        proto.random.Add(1.0f);
        proto.random.Add(2.0f);
        TestProto2 proto2 = SerializeUtil.FromProtoBuff<TestProto2>(SerializeUtil.ToProtoBuff(proto));
        Debug.Log(proto2.ID + ".." + proto2.name+".."+proto2.testEnum);
    }

}
public enum TestEnum
{
    A,
    B
}
public class TestJson
{
    public int ID;
    public string name;
    public List<float> random = new List<float>();
}
[ProtoContract]
public class TestProto
{
    [ProtoMember(1)]
    public int ID;
    [ProtoMember(2)]
    public string name;
    [ProtoMember(3)]
    public List<float> random = new List<float>();
    [ProtoMember(4)]
    public TestEnum testEnum = TestEnum.B;
}

[ProtoContract]
public class TestProto2
{
    [ProtoMember(1)]
    public int ID;
    [ProtoMember(2)]
    public string name;

    [ProtoMember(4)]
    public TestEnum testEnum;
}
