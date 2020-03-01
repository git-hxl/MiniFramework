using UnityEngine;
namespace MiniFramework
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract void Open();
        public abstract void Close();
        public abstract void Refresh();
    }
}