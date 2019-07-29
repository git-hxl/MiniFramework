using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public class HttpDownload
    {
        private string fileName;
        private string saveDir;
        private string filePath;
        private long curLength;
        private long totalLength;
        private float progress;
        private Action<bool> callback;
        public float Progress { get { return progress; } }
        public float CurLength { get { return curLength; } }
        public float TotalLength { get { return totalLength; } }
        public string FilePath { get { return filePath; } }
        public HttpDownload(string saveDir, Action<bool> callback = null)
        {
            this.saveDir = saveDir;
            this.callback = callback;
            FileUtil.CreateDir(saveDir);
        }
        public IEnumerator Download(string url)
        {
            fileName = url.Substring(url.LastIndexOf('/') + 1);
            filePath = saveDir + "/" + fileName;
            curLength = FileUtil.GetFileLength(filePath);
            using (UnityWebRequest headRequest = UnityWebRequest.Head(url))
            {
                yield return headRequest.SendWebRequest();
                if (headRequest.isHttpError || headRequest.isNetworkError)
                {
                    Debug.LogError(headRequest.error);
                    if (callback != null)
                        callback(false);
                    yield break;
                }
                totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));
            }
            if (curLength >= totalLength)
            {
                File.Delete(filePath);
                curLength = 0;
            }
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Range", "bytes=" + curLength + "-" + totalLength);
                request.SendWebRequest();
                Debug.Log("开始下载:" + fileName + " 大小:" + UnitConvert.ByteAutoConvert(totalLength));
                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
                        progress = request.downloadProgress;
                    }
                    Debug.Log("下载成功:" + fileName);
                    if (callback != null)
                        callback(true);
                }
            }
            yield return null;
        }
    }
}