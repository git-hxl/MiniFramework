using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public class HttpDownload
    {
        private string curUrl;
        private List<string> urls = new List<string>();
        private string fileName;
        private string saveDir;
        private string savePath;
        private long curLength;
        private long totalLength;
        private float progress;
        private MonoBehaviour mono;
        private Action callback;
        public bool IsCompleted;
        public int CompletedTask;
        public float GetProgress { get { return progress; } }
        public float GetCurLength { get { return curLength; } }
        public float GetTotalLength { get { return totalLength; } }
        public HttpDownload(MonoBehaviour mono, string url, string dir, Action callback = null)
        {
            urls.Add(url);
            this.mono = mono;
            this.saveDir = dir;
            this.callback = callback;
            FileUtil.CreateDir(dir);
            mono.StartCoroutine(Download());
        }

        public HttpDownload(MonoBehaviour mono, List<string> urls, string dir, Action callback = null)
        {
            this.mono = mono;
            this.urls = urls;
            this.saveDir = dir;
            this.callback = callback;
            FileUtil.CreateDir(dir);
            mono.StartCoroutine(Download());
        }
        IEnumerator Download()
        {
            foreach (var item in urls)
            {
                this.curUrl = item;
                yield return mono.StartCoroutine(GetEnumerator());
            }
            if (callback != null)
            {
                callback();
            }
            IsCompleted = true;
        }
        IEnumerator GetEnumerator()
        {
            fileName = curUrl.Substring(curUrl.LastIndexOf('/') + 1);
            savePath = saveDir + "/" + fileName;
            curLength = FileUtil.GetFileLength(savePath);
            using (UnityWebRequest headRequest = UnityWebRequest.Head(curUrl))
            {
                yield return headRequest.SendWebRequest();
                if (headRequest.isHttpError || headRequest.isNetworkError)
                {
                    Debug.LogError(headRequest.error);
                    yield break;
                }
                totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));
            }
            if (curLength >= totalLength)
            {
                File.Delete(savePath);
                curLength = 0;
            }
            using (UnityWebRequest request = UnityWebRequest.Get(curUrl))
            {
                request.SetRequestHeader("Range", "bytes=" + curLength + "-" + totalLength);
                request.SendWebRequest();
                Debug.Log("开始下载:" + fileName + " 大小:" + UnitConvert.ByteAutoConvert(totalLength));
                using (FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
                    CompletedTask++;
                    yield return null;
                    Debug.Log("下载成功:" + fileName);
                }
            }
        }
    }
}