using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Canvas mainCanvas;
        public Canvas MainCanvas { get { return mainCanvas; } }
        private Camera uiCamera;
        public Camera UiCamera { get { return uiCamera; } }
        private Dictionary<string, UIView> uiViews = new Dictionary<string, UIView>();
        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        private void Init()
        {
            mainCanvas = gameObject.GetComponentInChildren<Canvas>();
            if (mainCanvas == null)
            {
                Debug.LogError("缺少Canvas");
            }
            uiCamera = gameObject.GetComponentInChildren<Camera>();
            if (uiCamera == null)
            {
                Debug.LogError("缺少uiCamera");
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UIView Open(string assetPath)
        {
            UIView uiView = GetPanel(assetPath);
            if (uiView == null)
            {
                uiView = Load(assetPath);
            }
            if (uiView != null)
            {
                uiView.Open();

            }
            return uiView;
        }
        public T Open<T>(string assetPath) where T : UIView
        {
            return (T)Open(assetPath);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UIView Close(string assetPath)
        {
            UIView uiView = GetPanel(assetPath);
            if (uiView != null)
            {
                uiView.Close();
            }
            return uiView;
        }

        public void Destroy(string assetPath)
        {
            UIView uiView = GetPanel(assetPath);
            if (uiView != null)
            {
                Destroy(uiView.gameObject);
                uiViews.Remove(assetPath);
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UIView GetPanel(string assetPath)
        {
            UIView uiView;
            if (uiViews.TryGetValue(assetPath, out uiView))
            {
                if (uiView != null)
                    return uiView;
            }
            if (uiViews.ContainsKey(assetPath))
                uiViews.Remove(assetPath);
            return null;
        }

        private UIView Load(string assetPath)
        {
            if (uiViews.ContainsKey(assetPath))
                return null;
            GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>(assetPath);
            if (asset != null)
            {
                if (asset.GetComponent<UIView>() != null)
                {
                    Transform parent = mainCanvas.transform;
                    Canvas assetCanvas = asset.GetComponentInChildren<Canvas>();
                    if (assetCanvas != null)
                    {
                        parent = transform;
                        assetCanvas.worldCamera = uiCamera;
                    }
                    GameObject panelObj = Instantiate(asset, parent);
                    UIView uiView = panelObj.GetComponent<UIView>();
                    uiViews.Add(assetPath, uiView);
                    return uiView;
                }
            }
            Debug.LogError("UI加载失败：" + assetPath);
            return null;
        }

        public void Clear()
        {
            foreach (var item in uiViews)
            {
                if (item.Value != null)
                    Destroy(item.Value.gameObject);
            }
            uiViews.Clear();
            Resources.UnloadUnusedAssets();
        }

        public override void Dispose()
        {
            Clear();
            base.Dispose();
        }
    }
}