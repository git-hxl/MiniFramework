using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using ProtoBuf;
public class SerializeClass : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Test test = new Test();
        test.ID = 110;
        test.name = "xxoo";
        test.random = new double[] { 1 };

        string jsonData = SerializeUtil.ToJson(test);
        Debug.Log(jsonData);
        string xmlData = SerializeUtil.ToXml(test);
        Debug.Log(xmlData);
        SerializeUtil.ToXml(Application.streamingAssetsPath + "/test.xml", test);

        TestProto proto = new TestProto();
        proto.ID = 110;
        proto.name = "xxoo";
        proto.random.Add(1.0f);
        proto.random.Add(2.0f);
        TestProto2 proto2 = SerializeUtil.FromProtoBuff<TestProto2>(SerializeUtil.ToProtoBuff(proto));
        Debug.Log(proto2.ID + ".." + proto2.name + ".." + proto2.testEnum);
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
[ProtoContract]
public class TestProto
{
    [ProtoMember(1)]
    public int ID;
    [ProtoMember(2)]
    public string name;
    [ProtoMember(3)]
    public List<float> random;
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
