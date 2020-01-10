using UnityEngine;
using UnityEngine.UI;

namespace MiniFramework
{
    public abstract class BasePanel : MonoBehaviour
    {
        public abstract BasePanel Show();
        public abstract void Hide();
        public abstract void Destroy();
        /// <summary>
        /// 设置面板是否可交互
        /// </summary>
        /// <param name="panelName"></param>
        public void SetRayCast(bool value)
        {
            Image[] images = transform.GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                images[i].raycastTarget = value;
            }
            Text[] texts = transform.GetComponentsInChildren<Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].raycastTarget = value;
            }
        }
    }
}
