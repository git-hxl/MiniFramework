# MiniFramework
## 个人经验框架, 对常用功能进行了封装和简化,能够快速应用到项目中

1. UI
```
接口设计
IPanel:所有界面的基类
IUIManager:界面统一管理接口

常用接口示例:
UIManager.Instance.Open<UIRegister>();
//如果打开失败,会尝试从本地资源加载(发布版本无需处理资源地址,如下示例,默认会从AssetBundle中加载UIResigster.prefab)
UIManager.Instance.Open<UIRegister>("Assets/Example/01.UI/Prefabs/UIRegister.prefab");
```
2. Audio
```
接口设计
IAudioManager:音频统一管理接口

常用接口示例:
//从本地加载播放
AudioManager.Instance.PlaySound("Assets/Example/02.Audio/Audios/Click.wav");

AudioManager.Instance.PlaySoundAtPoint();

AudioManager.Instance.SetTotalVolume();

```
3. Resource
```
接口设计
IResourceManager : 资源加载统一管理接口
IResourceRead: 资源读取接口
IResourceUpdate:资源更新接口

常用接口示例:
//发布版本无需处理资源地址,如下示例,默认会从AssetBundle中加载UILogin.prefab,适用于编辑器模式。
GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>("Assets/Example/01.UI/Prefabs/UILogin.prefab");
//此方式只能从assetbundle中加载
GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>("UILogin");
Instantiate(asset,transform);
```
4.Pool
```
接口设计
IPool : 缓存池统一管理接口
IObjectPool:游戏对象缓存池统一管理接口
IPoolable: 缓存类必需接口

常用接口示例:
普通类：
TestPoolExample test = Pool<TestPoolExample>.Instance.Allocate();
Pool<TestPoolExample>.Instance.Recycle(test);o
游戏对象（该方式创建的游戏对象会挂载ObjectPoolable组件）
ObjectPool.Instance.Allocate("Assets/Example/04.Pool/PoolObject/Cube.prefab");
ObjectPool.Instance.Recycle(gameObject);
```
5.WebRequest
```
接口设计
IDownload : 资源下载接口,默认会采用断点续传
IWebRequestManager: WebRequest统一管理接口,封装了Get,Put,Post

常用接口示例:
//dir:下载保存路径 url:下载地址
WebRequestManager.Instance.Downloader(dir).Get(url);
WebRequestManager.Instance.Get(url,callback);
WebRequestManager.Instance.Put(url,data,callback);
WebRequestManager.Instance.Post(url,form,callback);
```
6.Network
```
```
7.Message
```
```