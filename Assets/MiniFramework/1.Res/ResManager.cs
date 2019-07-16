using System.Collections;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class ResManager : MonoSingleton<ResManager>
    {

        private string assetPath;
        public void Init(string platform)
        {
            assetPath = Application.persistentDataPath + "/" + platform;
        }
        // Use this for initialization
        IEnumerator Start()
        {
			DirectoryInfo dir = new DirectoryInfo(assetPath);
			dir.GetFiles();
			yield return AssetBundleLoader.LoadAssetBundle(assetPath);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public Object Load(string name,string assetBundle)
        {
            return AssetBundle.LoadFromFile(assetPath + "/" + assetBundle).LoadAsset(name);
        }
    }
}

