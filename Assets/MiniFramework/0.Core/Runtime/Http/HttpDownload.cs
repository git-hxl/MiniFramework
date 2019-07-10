using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public class HttpDownload
    {
        private string url;
        private string fileName;
        private string savePath;
        private long curLength;
        private long totalLength;
        private float progress;
        private MonoBehaviour mono;
        private Action callback;
        public float GetProgress { get { return progress; } }
        public float GetCurLength { get { return curLength; } }
        public float GetTotalLength { get { return totalLength; } }
        public HttpDownload(MonoBehaviour mono, string url, string dir, Action callback = null)
        {
            this.mono = mono;
            this.url = url;
            this.callback = callback;
            FileUtil.CreateDir(dir);
            fileName = url.Substring(url.LastIndexOf('/') + 1);
            savePath = dir + "/" + fileName;
            curLength = FileUtil.GetFileLength(savePath);
        }

        public void Download()
        {
            mono.StartCoroutine(GetEnumerator());
        }
        IEnumerator GetEnumerator()
        {
            using (UnityWebRequest headRequest = UnityWebRequest.Head(url))
            {
                yield return headRequest.SendWebRequest();
                if (headRequest.isHttpError || headRequest.isNetworkError)
                {
                    Debug.Log(headRequest.error);
                    yield break;
                }
                totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));
            }
            if (curLength >= totalLength)
            {
                File.Delete(savePath);
                curLength = 0;
            }
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Range", "bytes=" + curLength + "-" + totalLength);
                request.SendWebRequest();
                Debug.Log("开始下载:" + fileName + " 大小:" + UnitConvert.ByteAutoConvert(totalLength));
                using (FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.Seek(curLength, SeekOrigin.Begin);
                    int index = 0;
                    while (!request.isDone)
                    {
                        yield return null;
                        byte[] data = request.downloadHandler.data;
                        int writeLength = data.Length - index;
                        fileStream.Write(data, index, writeLength);
                        index = data.Length;
                        curLength += writeLength;
                        progress = (curLength / (float)totalLength);
                    }
                    if (callback != null)
                    {
                        callback();
                    }
                    Debug.Log("下载成功:" + fileName);
                }
            }
        }
    }
}