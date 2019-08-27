using UnityEngine;
using UnityEngine.UI;
using MiniFramework;
using LitJson;
using ProtoBuf;
using System.Net;

public class UILogin : MonoBehaviour
{
    public readonly string UserUrl = "https://adventure.spetchange.com/fishoauth/system/login?";
    public readonly string IP = /*"173.248.226.11";*/"adventure.spetchange.com";
    public readonly int port = 58010;

    public InputField Account;
    public InputField Password;

    public Button BtLogin;
    // Use this for initialization
    void Start()
    {
        MiniTcpClient.Instance.Connect(IP, port);
        Account.text = "SChange1";
        Password.text = "12345678";
        BtLogin.onClick.AddListener(() =>
        {
            WWWForm form = new WWWForm();
            form.AddField("username", Account.text);
            form.AddField("password", Password.text);
            form.AddField("thirdType", "PetCard");
            HttpRequest.Post(this, UserUrl, form, (string res) =>
            {
                Debug.Log(res);

                JsonData info = SerializeUtil.FromJson(res);
                if (info["code"].ToString() == "200")
                {
                    Player.HeadUrl = info["data"]["image_url"].ToString();
                    Player.Sex = (int)(info["data"]["sex"]);
                    Player.NickName = info["data"]["nickname"].ToString(); ;
                    Player.UserId = (int)(info["data"]["userId"]);

                    LoginRequest loginRequest = new LoginRequest();
                    loginRequest.ChannelId = 0;
                    loginRequest.Gender = Player.Sex;
                    loginRequest.OpenId = Player.UserId.ToString();
                    loginRequest.NickName = Player.NickName;
                    loginRequest.Avatar = Player.HeadUrl;
                    loginRequest.Platform = "PetCard";
                    MiniTcpClient.Instance.Send(10001, SerializeUtil.ToProtoBuff(loginRequest));
                }
            });

        });
        MsgDispatcher.Instance.Regist(this, 20001, (data) =>
        {
            LoginResponse loginResponse = SerializeUtil.FromProtoBuff<LoginResponse>((byte[])data[0]);
            if (loginResponse.Result == 0)
            {
                Debug.Log("登录成功:" + loginResponse.NickName);
            }
        });
        
        MsgDispatcher.Instance.Regist(this,30040,(data)=>{
            ScrollNoticesUpdate notices = SerializeUtil.FromProtoBuff<ScrollNoticesUpdate>((byte[])data[0]);
            foreach (var item in notices.notices)
            {
                Debug.Log(item.Content+":"+item.contentZh);
            }
        });
    }
}