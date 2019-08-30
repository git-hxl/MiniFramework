using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using ProtoBuf;
using System.Linq;

public class TestFrameClient : MonoBehaviour
{

    MoveData moveData = new MoveData();
    private float speed = 2;

    void Start()
    {
        Application.targetFrameRate = 50;
        MiniTcpClient.Instance.Connect("127.0.0.1", 8888);
        MsgDispatcher.Instance.Regist(this, 8888, (data) =>
        {
            MoveData recv = SerializeUtil.FromProtoBuff<MoveData>(data);
            Move(recv.keyCode);
        });
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.W))
        // {
        //     moveData.keyCode = KeyCode.W;
        //     MiniTcpClient.Instance.Send(8888, SerializeUtil.ToProtoBuff(moveData));
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     moveData.keyCode = KeyCode.A;
        //     MiniTcpClient.Instance.Send(8888, SerializeUtil.ToProtoBuff(moveData));
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     moveData.keyCode = KeyCode.S;
        //     MiniTcpClient.Instance.Send(8888, SerializeUtil.ToProtoBuff(moveData));
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     moveData.keyCode = KeyCode.D;
        //     MiniTcpClient.Instance.Send(8888, SerializeUtil.ToProtoBuff(moveData));
        // }

        // if (Input.GetKey(KeyCode.W))
        // {
        //     Move(KeyCode.W);
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     Move(KeyCode.A);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //      Move(KeyCode.S);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     Move(KeyCode.D);
        // }
        
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.S))
        {
             Move(KeyCode.S);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(KeyCode.D);
        }
    }

    void Move(KeyCode keyCode)
    {
        if (keyCode == KeyCode.W)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        if (keyCode == KeyCode.S)
        {
            transform.Translate(-Vector3.up * Time.deltaTime * speed);
        }
        if (keyCode == KeyCode.D)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (keyCode == KeyCode.A)
        {
            transform.Translate(-Vector3.right * Time.deltaTime * speed);
        }
    }
    [ProtoContract]
    public struct MoveData
    {
        [ProtoMember(1)]
        public KeyCode keyCode;
        [ProtoMember(2)]
        public int frame;
        [ProtoMember(3)]
        public float Time;
    }
}



