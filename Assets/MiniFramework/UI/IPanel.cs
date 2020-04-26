using UnityEngine;
namespace MiniFramework.UI
{
    public abstract class IPanel:MonoBehaviour
    {
        /// <summary>
        /// 打开界面
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// 关闭界面
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 刷新界面
        /// </summary>
        public abstract void Refresh();
    }
}