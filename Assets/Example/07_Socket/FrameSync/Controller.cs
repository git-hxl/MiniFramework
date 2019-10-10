using UnityEngine;
using MiniFramework;
using System.Text;

public class Controller : MonoBehaviour
{
    public float Speed;
    float x;
    float y;

    // Use this for initialization
    void Start()
    {
        MsgDispatcher.Instance.Regist(this, MsgID.Test, (data) =>
         {
              string[] s = Encoding.UTF8.GetString(data).Split('/');
              x = float.Parse(s[0]);
              y = float.Parse(s[1]);
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
            }
        }
        Move(x, y);
    }

    private void OnGUI()
    {
        GUILayout.Label("延迟：" + TimeoutChecker.Instance.NetWorkLatency * 1000 + "ms");
    }

    void Move(float x, float y)
    {
        if (x != 0 || y != 0)
            transform.Translate(x * Speed * Time.deltaTime, 0, y * Speed * Time.deltaTime);
    }
}
