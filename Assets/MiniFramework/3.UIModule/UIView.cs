using UnityEngine;
namespace MiniFramework
{
    public abstract class UIView:MonoBehaviour
    {
        /// <summary>
        /// 打开界面
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// 关闭界面
        /// </summary>
        public abstract void Close();

    }
}