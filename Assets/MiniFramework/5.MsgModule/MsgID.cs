namespace MiniFramework
{
    class MsgID
    {
        public const int ConnectFailed = -4;
        public const int ConnectSuccess = -3;
        public const int NetworkAbort = -2;
        public const int Test = -1;
        public const int HearBeat = 0;
        //public const int UserData = 1;


        // public const int Ack = 0;
        public const int Ping = 1;
        public const int Pong = 2;

        //public const int Login = 3;//登陆
        //public const int Logout = 4;//登出
        ////---------------你画我猜---------
        //public const int GameSync = 100;//状态同步

        //public const int GameReady = 101;//游戏准备
        //public const int GamePick = 102;//选择正确的玩家

    }
}
