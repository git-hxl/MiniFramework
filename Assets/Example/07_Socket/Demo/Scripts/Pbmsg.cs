using System.Collections.Generic;
using ProtoBuf;

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
    [ProtoMember(12)]
    public int SailingPack;
    [ProtoMember(13)]
    public int WeapongId;
    [ProtoMember(14)]
    public int NextLevelExp;
    [ProtoMember(15)]
    public int BeginnerCurTask;
    [ProtoMember(16)]
    public int BeginnerProgress;
    [ProtoMember(17)]
    public int PreferencepackBought;
    [ProtoMember(18)]
    public long LoginGold;
    [ProtoMember(19)]
    public string ErroMsg;
    [ProtoMember(20)]
    public int Gender;
    [ProtoMember(21)]
    public long Md5Key;
    [ProtoMember(22)]
    public int SignState;
    [ProtoMember(23)]
    public int RoleID;
    [ProtoMember(24)]
    public string Token;
    [ProtoMember(25)]
    public int RenameCount;
    [ProtoMember(26)]
    public string Platform;
    [ProtoMember(27)]
    public int NewBieGuideState;
}
[ProtoContract]
public class ScrollNoticesUpdate
{
    [ProtoContract]
    public class ScrollNtoice
    {
        [ProtoMember(1)]
        public string Content;
        [ProtoMember(2)]
        public int cycleInterval;
        [ProtoMember(3)]
        public string contentZh;
    }
    [ProtoMember(1)]
    public List<ScrollNtoice> notices;
}
