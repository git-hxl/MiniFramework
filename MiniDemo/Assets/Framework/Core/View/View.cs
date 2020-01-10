using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class View : MonoSingleton<View>
    {
        public Canvas UICanvas;
        private Dictionary<string, BasePanel> UIPanelDict = new Dictionary<string, BasePanel>();
        public BasePanel OpenUI(string name)
        {
            BasePanel panel;
            UIPanelDict.TryGetValue(name, out panel);
            if (panel != null)
            {
                panel.Show();
            }
            else
            {
                panel = LoadUI(name);
            }
            return panel;
        }
        public void CloseUI(string name)
        {
            if (UIPanelDict.ContainsKey(name))
            {
                UIPanelDict[name].Hide();
            }
        }
        public void DestroyUI(string name)
        {
            if (UIPanelDict.ContainsKey(name))
            {
                UIPanelDict[name].Destroy();
            }
        }

        private BasePanel LoadUI(string name)
        {
            GameObject ui = ResourceManager.Instance.Load<GameObject>(name);
            BasePanel panel = Instantiate(ui, UICanvas.transform).GetComponent<BasePanel>();
            UIPanelDict[name] = panel;
            return panel;
        }
    }
}