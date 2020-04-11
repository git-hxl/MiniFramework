using System;
using System.Collections;
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
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Get complete!");
                    callback(www.downloadHandler.text);
                }
            }
        }
        public static void Get(this MonoBehaviour mono, string url, Action<Texture2D> callback)
        {
            mono.StartCoroutine(GetEnumerator(url, callback));
        }
        static IEnumerator GetEnumerator(string url, Action<Texture2D> callback)
        {
            using (UnityWebRequest www =  UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Get complete!");
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    callback(texture);
                }
            }
        }
        public static void Put(this MonoBehaviour mono, string url, byte[] data, Action<string> callback)
        {
            mono.StartCoroutine(PutEnumerator(url, data, callback));
        }
        static IEnumerator PutEnumerator(string url, byte[] data, Action<string> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Put(url, data))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Upload complete!");
                    callback(www.downloadHandler.text);
                }
            }
        }
        public static void Post(this MonoBehaviour mono, string url, WWWForm form, Action<string> callback)
        {
            mono.StartCoroutine(PostEnumerator(url, form, callback));
        }
        static IEnumerator PostEnumerator(string url, WWWForm form, Action<string> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    callback(www.downloadHandler.text);
                }
            }
        }

    }
}