using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class CheckUpdate : MonoBehaviour
    {
        public Dictionary<string, string> Version = new Dictionary<string, string>();
        // Use this for initialization
        void Start()
        {
            GetVersion();
            Debug.Log(Application.dataPath);
        }

        void GetVersion()
        {
            if (!FileUtil.IsExitFile(Application.streamingAssetsPath + "/Config.txt"))
            {
                Debug.LogError("Config文件不存在");
            }

        }
    }
}