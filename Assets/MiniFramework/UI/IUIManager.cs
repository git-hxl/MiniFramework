namespace MiniFramework.UI
{
    public interface IUIManager
    {
        /// <summary>
        /// 添加界面
        /// </summary>
        void AddPanel(IPanel IPanel);
        /// <summary>
        /// 移除界面
        /// </summary>
        void RemovePanel(IPanel IPanel);
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Open<T>() where T : IPanel;
        /// <summary>
        /// 加载界面并打开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Open<T>(string name) where T : IPanel;
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Close<T>() where T : IPanel;
        /// <summary>
        /// 获取界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>() where T : IPanel;
        /// <summary>
        /// 卸载所有界面资源
        /// </summary>
        void UnloadAll();
    }
}