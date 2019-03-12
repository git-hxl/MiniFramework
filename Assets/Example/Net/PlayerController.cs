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
    private bool needLerp;
    private bool isDrag;
    private Vector3 needLerpPos;
    PackHead head;
    Msg msg;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        joyStick = FindObjectOfType<JoyStick>();
         head = new PackHead();
        head.MsgID = 10001;
         msg = new Msg();
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

                isDrag = true;
            };
            joyStick.OnDragEnd += () =>
            {
                msg.state = 3;
                msg.dir = transform.position;
                byte[] data = Binary.SerializeByMarshal(msg);
                SocketManager.Instance.SendToServer(head, data);
                isDrag = false;
            };
        }
        if (!IsSelf)
        {
            //非本机用户 从服务端获取输入信息
            MsgManager.Instance.RegisterMsg(this, "10001", (data) =>
            {
                Msg msg2 = Binary.DeserializeByMarshal<Msg>((byte[])data);
                needLerp = false;
                switch (msg2.state)
                {
                    case 1:
                        transform.position = msg2.dir;

                        break;
                    case 2:
                        dir = msg2.dir;
                        break;
                    case 3:
                        dir = Vector2.zero;
                        if (Vector3.Distance(msg2.dir, transform.position) > 1)
                        {
                            needLerpPos = msg2.dir;
                            needLerp = true;
                        }
                        break;
                }

            });
        }
        Camera.main.GetComponent<CameraFollow>().Player = transform;
    }
    void Update()
    {
        if (needLerp)
        {
            transform.LookAt(needLerpPos);
            transform.position = Vector3.Lerp(transform.position, needLerpPos, Time.deltaTime);
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

        if (isDrag)
        {
            msg.state = 2;
            Vector3 dirSend = new Vector3(joyStick.RockerDir.x, 0, joyStick.RockerDir.y);
            msg.dir = dirSend;
            byte[] bytes = Binary.SerializeByMarshal(msg);
            SocketManager.Instance.SendToServer(head, bytes);
        }
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