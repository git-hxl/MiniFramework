using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public class HttpManager : Singleton<HttpManager>
    {
        public IEnumerator Get(string url, Action<DownloadHandler> callback, Action erro = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                    erro?.Invoke();
                }
                else
                {
                    Debug.Log("Get complete!");
                    callback?.Invoke(www.downloadHandler);
                }
            }
        }

        public IEnumerator Get(string url, Action<Sprite> callback)
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
                    Texture2D texture = new Texture2D(0, 0);
                    texture.LoadImage(www.downloadHandler.data);
                    Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    callback?.Invoke(sp);
                }
            }
        }
        public IEnumerator Post(string url, WWWForm form, Action<DownloadHandler> callback, Action erro = null)
        {
            Debug.Log(url);
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                    erro?.Invoke();
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    callback?.Invoke(www.downloadHandler);
                }
            }
        }
        public IEnumerator Put(string url, byte[] data, Action<DownloadHandler> callback)
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
                    callback?.Invoke(www.downloadHandler);
                }
            }
        }
    }
}