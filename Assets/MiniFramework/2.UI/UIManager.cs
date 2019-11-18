using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private readonly Dictionary<string, GameObject> UIPanelDict = new Dictionary<string, GameObject>();
        private Canvas m_Canvas;
        void Start()
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
        public GameObject OpenUI(string panelName)
        {
            GameObject panel = null;
            if (UIPanelDict.ContainsKey(panelName))
            {
                panel = UIPanelDict[panelName];
                panel.SetActive(true);
            }
            else
            {
                GameObject asset = ResourceManager.Instance.Load<GameObject>(panelName);
                if (asset != null)
                {
                    panel = Instantiate(asset, m_Canvas.transform) as GameObject;
                    panel.SetActive(true);
                    panel.name = asset.name;
                    UIPanelDict.Add(panel.name, panel);
                }
            }
            if (panel == null)
            {
                Debug.LogError(panelName + "不存在");
            }
            return panel;
        }
        public void CloseUI(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject up = UIPanelDict[panelName];
                up.SetActive(false);
            }
        }
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
