using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;

public class Controller : MonoBehaviour
{
    public float Speed;

    float x;
    float y;

    private float timeout;
    private float sendtime;
    private float recvtime;

    // Use this for initialization
    void Start()
    {
        MsgDispatcher.Instance.Regist(this, MsgID.Test, (data) =>
         {
             string[] s = Encoding.UTF8.GetString(data).Split('/');
             x = float.Parse(s[0]);
             y = float.Parse(s[1]);
             recvtime = Time.time;

             timeout = recvtime - sendtime;
         });
    }

    // Update is called once per frame
    void Update()
    {
        float sendx = Input.GetAxisRaw("Horizontal");
        float sendy = Input.GetAxisRaw("Vertical");
        if (MiniTcpClient.Instance.IsConnected)
        {

            if (sendx != x || sendy != y)
            {
                string s = sendx + "/" + sendy;
                MiniTcpClient.Instance.Send(MsgID.Test, Encoding.UTF8.GetBytes(s));
                sendtime = Time.time;
            }
        }
        Move(x, y);
    }

    private void OnGUI()
    {
        GUILayout.Label("延迟：" + timeout * 1000 + "ms");
    }

    void Move(float x, float y)
    {
        transform.Translate(x * Speed * Time.deltaTime, 0, y * Speed * Time.deltaTime);
    }
}
