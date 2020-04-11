# MiniFramework
## 个人经验框架, 对常用功能进行了封装和简化,能够快速应用到项目中,0学习成本

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