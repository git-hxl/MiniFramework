using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Resource;
namespace MiniFramework.UI
{
    public sealed class UIManager : MonoSingleton<UIManager>, IUIManager
    {
        private Canvas canvas;

        private List<IPanel> uIPanels;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        private void Init()
        {
            canvas = GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas不能为null");
            }
            uIPanels = new List<IPanel>();
        }
        /// <summary>
        /// 添加界面
        /// </summary>
        public void AddPanel(IPanel IPanel)
        {
            if (!uIPanels.Contains(IPanel))
            {
                uIPanels.Add(IPanel);
            }
        }
        /// <summary>
        /// 移除界面
        /// </summary>
        /// <param name="IPanel"></param>
        public void RemovePanel(IPanel IPanel)
        {
            if (uIPanels.Contains(IPanel))
            {
                uIPanels.Remove(IPanel);
            }
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Close<T>() where T : IPanel
        {
            foreach (var item in uIPanels)
            {
                if (item.GetType() is T)
                {
                    item.Close();
                    return (T)item;
                }
            }
            return default(T);
        }
        /// <summary>
        /// 获取界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : IPanel
        {
            foreach (var item in uIPanels)
            {
                if (item.GetType() is T)
                {
                    return (T)item;
                }
            }
            return default(T);
        }
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Open<T>() where T : IPanel
        {
            foreach (var item in uIPanels)
            {
                if (item is T)
                {
                    item.Open();
                    item.transform.SetAsLastSibling();
                    return (T)item;
                }
            }
            return default(T);
        }
        /// <summary>
        /// 加载界面并打开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Open<T>(string name) where T : IPanel
        {
            T panel = Open<T>();
            if (panel != null)
            {
                return panel;
            }
            GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>(name);
            if (asset != null)
            {
                GameObject view = Instantiate(asset, canvas.transform);
                view.name = asset.name;
                T t = view.GetComponent<T>();
                t.Open();
                AddPanel(t);
                return t;
            }
            return null;
        }

        /// <summary>
        /// 卸载所有界面资源
        /// </summary>
        public void UnloadAll()
        {
            uIPanels.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}