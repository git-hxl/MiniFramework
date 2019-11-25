# MiniFramework
U3D客户端框架

一、消息模块
* 主线程消息通信
```
//注册
GameMsgManager.Instance.Regist<string>(GameMsgID.Test, OnRecv);
//回调
void OnRecv(string msg)
{
	Debug.Log(msg);
}
//撤销注册
GameMsgManager.Instance.UnRegist<string>(GameMsgID.Test, OnRecv);
//发送
GameMsgManager.Instance.Dispatch<string>(GameMsgID.Test, "hello");
```
* 网络消息通信
```
//注册
NetMsgManager.Instance.Regist(NetMsgID.Test, OnRecv);
//回调
void OnRecv(byte[] data)
{
       
}
//撤销注册
NetMsgManager.Instance.UnRegist(NetMsgID.Test, OnRecv);
//发送
NetMsgManager.Instance.Dispatch(NetMsgID.Test, Encoding.UTF8.GetBytes("Hello"));
```

二、事件模块
```
//延迟事件
DelayAction.Excute(this, 2, () => Debug.Log("2s执行"));
//条件事件
UntilAction.Excute(this, () => Input.GetKeyDown(KeyCode.Space), () => Debug.Log("按下Space"));
//重复事件
RepeatAction.Excute(this, 5, -1, () => Debug.Log("每隔5秒 重复无限次"));
//事件链
ActionChain chain = new ActionChain(this);
chain.Delay(5, () => Debug.Log("hello"))
.Until(() => Input.GetKeyDown(KeyCode.Space), () => Debug.Log("Space"))
.Repeat(2, 2, () => Debug.Log("每隔2秒 重复2次"))
.Excute();
```
三、序列化模块
```
//自定义CSV解析（json、protobuf采用了第三方库）
CSVData cSVData = CSVUtil.FromCSV(Csv.text);
Debug.Log(cSVData[1]["Level"]);
```
四、下载模块
```
//采用协程下载（支持断点续传）
HttpDownload httpDownload = new HttpDownload(Application.dataPath);
yield return httpDownload.Download(url,Callback);
//可依次下载多个任务
yield return httpDownload.Download(url2,Callback);
```
五、资源加载
```
//同时支持Resource、编辑器内和AB包
ResourceManager.Instance.Load<Sprite>("Background");
```