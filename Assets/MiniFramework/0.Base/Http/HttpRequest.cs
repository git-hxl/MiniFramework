using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
namespace MiniFramework
{
    public static class HttpRequest
    {
        public static void Get(this MonoBehaviour mono, string url, Action<string> callback)
        {
            mono.StartCoroutine(GetEnumerator(url, callback));
        }
        static IEnumerator GetEnumerator(string url, Action<string> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Get complete!");
                    callback(www.downloadHandler.text);
                }
            }
        }
        public static void Put(this MonoBehaviour mono, string url, byte[] data, Action<bool> callback)
        {
            mono.StartCoroutine(PutEnumerator(url, data, callback));
        }
        static IEnumerator PutEnumerator(string url, byte[] data, Action<bool> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Put(url, data))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    callback(false);
                }
                else
                {
                    Debug.Log("Upload complete!");
                    callback(true);
                }
            }
        }
        public static void Post(this MonoBehaviour mono, string url, WWWForm form, Action<bool> callback)
        {
            mono.StartCoroutine(PostEnumerator(url, form, callback));
        }
        static IEnumerator PostEnumerator(string url, WWWForm form, Action<bool> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    callback(false);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    callback(true);
                }
            }
        }
        public static void Download(this MonoBehaviour mono, string url, string dir, Action<float> callback)
        {
            mono.StartCoroutine(GetEnumerator(url, dir, callback));
        }
        static IEnumerator GetEnumerator(string url, string dir, Action<float> callback)
        {
            UnityWebRequest headRequest = UnityWebRequest.Head(url);
            yield return headRequest.SendWebRequest();

            long totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));
            string fileName = url.Substring(url.LastIndexOf('/') + 1);
            string savePath = dir + "/" + fileName;
            long curLength = FileUtil.GetFileLength(savePath);
            if (curLength >= totalLength)
            {
                Debug.LogWarning(fileName + ":已经下载完成！");
                yield break;
            }
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                www.SetRequestHeader("Range", "bytes=" + curLength + "-" + totalLength);
                www.timeout = 10000;
                www.SendWebRequest();
                Debug.Log("开始下载:" + fileName + ":" + UnitConvert.ByteAutoConvert(totalLength));
                using (FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.Seek(curLength, SeekOrigin.Begin);
                    int index = 0;
                    while (!www.isDone)
                    {
                        yield return null;
                        byte[] data = www.downloadHandler.data;
                        int writeLength = data.Length - index;
                        fileStream.Write(data, index, writeLength);
                        index = data.Length;
                        curLength += writeLength;
                        if (callback != null)
                        {
                            float progress = (curLength / (float)totalLength);
                            callback(progress);
                        }
                    }
                    Debug.Log("下载成功:" + fileName);
                }
            }
        }
    }
}

