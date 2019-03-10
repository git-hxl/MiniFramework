using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniFramework
{
    public class UIManager: MonoSingleton<UIManager>
    {
        public string UIDownloadPath = Application.streamingAssetsPath + "/AssetBundle/StandaloneWindows/ui";
        public string UIResourecePath = "UI";       
        private readonly Dictionary<string, GameObject> UIPanelDict = new Dictionary<string, GameObject>();
        protected override void OnSingletonInit()
        {
            
        }
        public void Start()
        {
            ResourceManager.Instance.AssetLoader.LoadAssetBundles(UIDownloadPath,LoadCallback);
        }
        void LoadCallback(Object[] objs){
            foreach (var item in objs)
            {
               GameObject obj =  Instantiate(item,transform) as GameObject;
               UIPanelDict.Add(obj.name,obj);
            }
        }
        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="panelName"></param>
        public void OpenUI(string panelName)
        {
            if (UIPanelDict.ContainsKey(panelName))
            {
                GameObject up = UIPanelDict[panelName];
                up.SetActive(true);
            }
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
