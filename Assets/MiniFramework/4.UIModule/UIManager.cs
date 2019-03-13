﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public string UIDownloadPath;
        public string UIResourecePath;
        private readonly Dictionary<string, GameObject> UIPanelDict = new Dictionary<string, GameObject>();
        private Canvas canvas;
        protected override void OnSingletonInit()
        {
            canvas = GetComponentInChildren<Canvas>();
        }
        public void Start()
        {
            if (canvas != null)
            {
                for (int i = 0; i < canvas.transform.childCount; i++)
                {
                    GameObject child = canvas.transform.GetChild(i).gameObject;
                    UIPanelDict.Add(child.name, child);
                }
            }
            if (UIDownloadPath != "")
            {
                ResourceManager.Instance.AssetLoader.LoadAssetBundles(UIDownloadPath, LoadCallback);
            }
            if (UIResourecePath != "")
            {
                ResourceManager.Instance.AssetLoader.LoadAllAsset(UIResourecePath, LoadCallback);
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
        public GameObject GetUI(string panelName){
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject ui = UIPanelDict[panelName];
                return ui;
            }
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
                up.SetActive(false);
            }
        }
        public void CloseUIByAnimation(string panelName){
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject up = UIPanelDict[panelName];
                up.GetComponent<Animator>().Play("close");
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
