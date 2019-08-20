using UnityEngine;
using UnityEngine.UI;
using MiniFramework;
using LitJson;
using ProtoBuf;

public class UILogin : MonoBehaviour
{
    public readonly string UserUrl = "https://adventure.spetchange.com/fishoauth/system/login?";
    public readonly string Ip = "adventure.spetchange.com";
    public readonly int port = 58010;

    public InputField Account;
    public InputField Password;

    public Button BtLogin;
    // Use this for initialization
    void Start()
    {
        Account.text = "SChange9";
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
                    MiniTcpClient.Instance.Send(10001,SerializeUtil.ToProtoBuff(loginRequest));
                }
            });

        });

        MiniTcpClient.Instance.Connect(Ip, port);
		MsgManager.Instance.RegisterMsg(this,20001,(data)=>{
			LoginResponse loginResponse = SerializeUtil.FromProtoBuff<LoginResponse>((byte[])data);
			if(loginResponse.Result ==0)
			{
				Debug.Log("登录成功:"+loginResponse.NickName);
			}
		});

    }
}
[ProtoContract]
public class LoginRequest
{
    [ProtoMember(1)]
    public int Gender;
    [ProtoMember(2)]
    public string OpenId;
    [ProtoMember(3)]
    public string NickName;
    [ProtoMember(4)]
    public string Avatar;
    [ProtoMember(5)]
    public int ChannelId;
    [ProtoMember(6)]
    public string Account;
    [ProtoMember(7)]
    public int Language;
    [ProtoMember(8)]
    public string Platform;
}

[ProtoContract]
public class LoginResponse
{
    [ProtoMember(1)]
    public int Result;
    [ProtoMember(2)]
    public string NickName;
    [ProtoMember(3)]
    public string Avatar;
    [ProtoMember(4)]
    public int VipLevel;
    [ProtoMember(5)]
    public int Level;
    [ProtoMember(6)]
    public int Experience;
    [ProtoMember(7)]
    public long UserId;
    [ProtoMember(8)]
    public long Glod;
	[ProtoMember(9)]
    public long Diamond;
	[ProtoMember(10)]
    public int TopupSum;
	[ProtoMember(11)]
    public int MaxWeaponMultiple;
}