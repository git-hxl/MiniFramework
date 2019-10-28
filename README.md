# MiniFramework
U3D客户端框架

一、消息模块
主线程消息通信
```
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
```
子线程消息同步到主线程
```
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
```

二、事件模块
```
//延迟事件
DelayAction.Excute(this, 2, () => Debug.Log(222222));
//条件事件
UntilAction.Excute(this, () => Input.GetKeyDown(KeyCode.Space), () => Debug.Log("Space"));
//重复事件
RepeatAction.Excute(this, 1, -1, () => Debug.Log("repeat 1"));
//事件链
ActionChain chain = new ActionChain(this);
chain.Delay(5, () => Debug.Log(555555))
.Until(() => Input.GetKeyDown(KeyCode.W), () => Debug.Log("W"))
.Repeat(2, 2, () => Debug.Log("repeat 2"))
.Excute();
```
三、序列化模块
```
//支持第三方Json、Protobuf解析，自定义CSV解析
CSVData cSVData = CSVUtil.FromCSV(Csv.text);
Debug.Log(cSVData[1]["Level"]);
```
四、下载模块
```
//采用协程下载
HttpDownload httpDownload = new HttpDownload(Application.dataPath);
yield return httpDownload.Download(url,Callback);
//可依次下载多个任务
yield return httpDownload.Download(url2,Callback);
```