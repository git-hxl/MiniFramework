using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public List<UIPanel> uIPanels = new List<UIPanel>();

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        void Init()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                UIPanel panel = transform.GetChild(i).GetComponent<UIPanel>();
                if (panel != null)
                {
                    uIPanels.Add(panel);
                }
            }
        }

        public UIPanel Open(string name)
        {
            foreach (var item in uIPanels)
            {
                if (item.name == name)
                {
                    item.Open();
                    item.transform.SetAsLastSibling();
                    return item;
                }
            }

            GameObject asset = ResManager.Instance.Load<GameObject>(name);
            if (asset != null)
            {
                UIPanel panel = Instantiate(asset, transform).GetComponent<UIPanel>();
                panel.name = name;
                uIPanels.Add(panel);
                return panel;
            }

            Debug.LogError("UIPanel:" + name + "不存在");
            return null;
        }

        public UIPanel GetPanel(string name)
        {
            foreach (var item in uIPanels)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
            Debug.LogError("UIPanel:" + name + "不存在");
            return null;
        }

    }
}