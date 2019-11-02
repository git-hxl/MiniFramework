using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class UpdateComponent : MonoBehaviour
    {
        public string ResUrl;
        public string Platform;
        public string Version;

        private HttpDownload httpDownload;
        private List<string> updateFiles = new List<string>();
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator CheckConfig()
        {
            string configPath = Application.streamingAssetsPath + "/config.txt";
            yield return FileUtil.ReadStreamingFile(configPath, (result) =>
            {
                LitJson.JsonData jsonData = SerializeUtil.FromJson(result);
                Platform = jsonData["platform"].ToString();
                Version = jsonData["version"].ToString();
                Debug.Log("当前版本号:" + Version + " 平台:" + Platform);
            });


        }

        void CheckVersion()
        {
            string url = ResUrl + "/" + "/Plaform" + "/config.txt";
        }
    }
}