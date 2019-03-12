using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class PlayerController : MonoBehaviour
{
    public Vector3 dir;
    public float Speed;
    public bool IsSelf;
    public string Port;
    private JoyStick joyStick;
    private new Rigidbody rigidbody;
    private bool isStop;
    private Vector3 needLerpPos;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        joyStick = FindObjectOfType<JoyStick>();
        PackHead head = new PackHead();
        head.MsgID = 10001;
        Msg msg = new Msg();
        if (IsSelf)
        {
            joyStick.OnDragStart += () =>
            {
                msg.state = 1;
                msg.dir = transform.position;
                byte[] data = Binary.SerializeByMarshal(msg);
                SocketManager.Instance.SendToServer(head, data);
            };
            joyStick.OnDragging += (data) =>
            {
                msg.state = 2;
                Vector3 dirSend = new Vector3(data.x,0,data.y);
                msg.dir = dirSend;
                byte[] bytes = Binary.SerializeByMarshal(msg);
                SocketManager.Instance.SendToServer(head, bytes);
            };
            joyStick.OnDragEnd += () =>
            {
                msg.state = 3;
                msg.dir = transform.position;
                byte[] data = Binary.SerializeByMarshal(msg);
                SocketManager.Instance.SendToServer(head, data);
            };
        }


        if (!IsSelf)
        {
            //非本机用户 从服务端获取输入信息
            MsgManager.Instance.RegisterMsg(this, "10001", (data) =>
            {
                Msg msg2 = Binary.DeserializeByMarshal<Msg>((byte[])data);
                isStop = false;
                switch (msg2.state)
                {
                    case 1:
                        transform.position = msg2.dir;
                        
                        break;
                    case 2:
                        dir = msg2.dir;
                        Debug.Log("接受到方向"+msg.dir);
                        break;

                    case 3:
                        dir = Vector3.zero;
                        //transform.position = msg2.dir;
                        Debug.Log("移动结束" + msg2.dir);
                        isStop =true;
                        needLerpPos = msg2.dir;
                        break;
                }

            });
        }
        Camera.main.GetComponent<CameraFollow>().Player = transform;
    }
    void Update()
    {
        if(isStop&&needLerpPos!=Vector3.zero){
            transform.LookAt(needLerpPos);
            transform.position = Vector3.MoveTowards(transform.position,needLerpPos,Time.deltaTime*Speed);
        }
    }
    void FixedUpdate()
    {
        if (IsSelf)
        {
            //本地玩家从摇杆直接获取数据
            dir.x = joyStick.RockerDir.x;
            dir.z = joyStick.RockerDir.y;
        }

        MoveAndRoate(dir);
    }
    void MoveAndRoate(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
        rigidbody.velocity = transform.forward * dir.magnitude * Speed;
    }
}


public struct Msg
{
    public int state;
    public Vector3 dir;
}