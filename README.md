# MiniFramework
U3D客户端框架

# 一、消息模块
##	主线程消息通信
''' C#
	//注册
	GameMsgDispatcher.Instance.Regist<string>(GameMsgID.Test, OnRecv);
	//回调
	void OnRecv(string msg)
    {
        Debug.Log(msg);
    }
	//撤销注册
	GameMsgDispatcher.Instance.UnRegist<string>(GameMsgID.Test, OnRecv);
	//发送
	GameMsgDispatcher.Instance.Dispatch<string>(GameMsgID.Test, "hello");
'''
##	子线程消息同步到主线程
''' C#
	//注册
	NetMsgDispatcher.Instance.Regist(NetMsgID.Test, OnRecv);
	//回调
	void OnRecv(byte[] data)
    {
        
    }
	//撤销注册
	NetMsgDispatcher.Instance.UnRegist(NetMsgID.Test, OnRecv);
	//发送
	NetMsgDispatcher.Instance.Dispatch(NetMsgID.Test, Encoding.UTF8.GetBytes("Hello"));
'''
#二、事件