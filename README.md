# MiniFramework
## 个人经验框架, 对常用功能进行了封装和简化,能够快速应用到项目中

1. UI
```
接口设计
IPanel:所有界面的基类
IUIManager:界面统一管理接口

常用接口示例:
UIManager.Instance.Open<UIRegister>();
//如果打开失败,会尝试从本地资源加载
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
3. Resouce
```
接口设计
IResourceManager : 资源加载统一管理接口
IResourceLoad: 资源读取接口
IResourceUpdate:资源更新接口

常用接口示例:
GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>("UILogin");
Instantiate(asset,transform);
```