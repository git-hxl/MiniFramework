# MiniFramework
## 个人经验框架, 对常用功能进行了封装和简化,能够快速应用到项目中

1. UI
```
//继承接口IPanel
public class UILogin : IPanel
{}
//打开已缓存的界面
UIManager.Instance.Open<UIRegister>();
//加载界面并打开,如果已缓存则不会加载
UIManager.Instance.Open<UIRegister>("Assets/Example/01.UI/Prefabs/UIRegister.prefab");
```
2. Audio
```
//播放音效 如果没有缓存会去加载
AudioManager.Instance.PlaySound("Assets/Example/02.Audio/Audios/Click.wav");

AudioManager.Instance.PlaySoundAtPoint();

AudioManager.Instance.SetTotalVolume();

```
3. Resouce
```
接口设计
IResourceManager : Resouce统一管理单列脚本 用于加载资源
IResourceLoad: 用于读取本地资源镜像
IResourceUpdate:用于更新资源,包含差异化对比,断点下载,下载进度查看
```