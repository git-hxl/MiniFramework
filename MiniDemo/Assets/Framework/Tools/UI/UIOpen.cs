using UnityEngine;
using UnityEngine.UI;
namespace MiniFramework
{
    public class UIOpen : MonoBehaviour
    {
        public string Path;
        private Button button;
        // Use this for initialization
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                View.Instance.OpenUI(Path);
            });
        }
    }
}