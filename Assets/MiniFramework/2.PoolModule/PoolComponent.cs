using UnityEngine;
namespace MiniFramework
{
    public class PoolComponent : MonoBehaviour
    {
        public string assetPath;
        public int maxCount;
        // Start is called before the first frame update
        void Start()
        {
            PoolManager.Instance.Add(assetPath,maxCount);
        }
    }
}