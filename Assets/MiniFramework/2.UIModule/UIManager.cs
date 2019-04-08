using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public string UIDownloadPath;
        private readonly Dictionary<string, GameObject> UIPanelDict = new Dictionary<string, GameObject>();
        private Canvas m_Canvas;
        private Camera m_Camera;
        protected override void OnSingletonInit()
        {
            m_Canvas = GetComponentInChildren<Canvas>();
            if (m_Canvas != null)
            {
                for (int i = 0; i < m_Canvas.transform.childCount; i++)
                {
                    GameObject child = m_Canvas.transform.GetChild(i).gameObject;
                    UIPanelDict.Add(child.name, child);
                }
            }
        }
        public void Start()
        {
            if (!string.IsNullOrEmpty(UIDownloadPath))
            {
                AssetBundleHelper.Instance.LoadAssetBundles(UIDownloadPath, LoadCallback);
            }
        }
        void LoadCallback(Object[] objs)
        {
            foreach (var item in objs)
            {
                GameObject obj = Instantiate(item, transform) as GameObject;
                obj.name = item.name;
                UIPanelDict.Add(obj.name, obj);
            }
        }
        public GameObject GetUI(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject ui = UIPanelDict[panelName];
                return ui;
            }
            Debug.LogError(panelName + "不存在");
            return null;
        }
        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="panelName"></param>
        public GameObject OpenUI(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject ui = UIPanelDict[panelName];
                ui.SetActive(true);
                return ui;
            }
            Debug.LogError(panelName + "不存在");
            return null;
        }
        /// <summary>
        /// 从Resource中打开UI
        /// </summary>
        /// <param name ="path"></param>
        public GameObject LoadUI(string path)
        {
            Object ui = Resources.Load(path);
            if (ui != null)
            {
                GameObject obj = Instantiate(ui, transform) as GameObject;
                obj.name = ui.name;
                UIPanelDict.Add(obj.name, obj);
                return obj;
            }
            Debug.LogError(path + "不存在");
            return null;
        }
        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="panelName"></param>
        public void CloseUI(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject up = UIPanelDict[panelName];
                Animator animator = up.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("close");
                }
                else
                {
                    up.SetActive(false);
                }
            }
        }
        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="panelName"></param>
        public void DestroyUI(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                Destroy(UIPanelDict[panelName].gameObject);
                UIPanelDict.Remove(panelName);
            }
        }
        /// <summary>
        /// 禁用面板交互
        /// </summary>
        /// <param name="panelName"></param>
        public void DisableRayCast(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject panel = UIPanelDict[panelName];
                Image[] images = panel.transform.GetComponentsInChildren<Image>();
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].raycastTarget = false;
                }
                Text[] texts = panel.transform.GetComponentsInChildren<Text>();
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].raycastTarget = false;
                }
            }
        }
        /// <summary>
        /// 启用面板交互
        /// </summary>
        /// <param name="panelName"></param>
        public void EnableRayCast(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject panel = UIPanelDict[panelName];
                Image[] images = panel.transform.GetComponentsInChildren<Image>();
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].raycastTarget = true;
                }
                Text[] texts = panel.transform.GetComponentsInChildren<Text>();
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].raycastTarget = true;
                }
            }
        }
    }
}
