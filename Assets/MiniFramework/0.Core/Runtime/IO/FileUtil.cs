using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MiniFramework
{
    public static class FileUtil
    {
        /// <summary>
        /// 采用,分割符号读取，使用“,”可保留,分割符号
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<int, string[]> ReadCSV(string path)
        {
            string txts = ReadFile(path);
            Dictionary<int, string[]> csv = new Dictionary<int, string[]>();
            try
            {
                string[] txt2Array = txts.Split('\n');
                for (int i = 1; i < txt2Array.Length; i++)
                {
                    List<string> txt = txt2Array[i].Split(',').ToList();
                    int id = int.Parse(txt[0]);
                    txt.RemoveAt(0);
                    int head = 0;
                    for (int j = 0; j < txt.Count; j++)
                    {
                        txt[j] = txt[j].Replace("\\n", "\n");
                        if (txt[j].IndexOf("\"") == 0)
                        {
                            head = j;
                            continue;
                        }
                        if (txt[j].IndexOf("\"") == txt[j].Length - 1)
                        {
                            txt[head] = (txt[head] + "," + txt[j]).Trim('"');
                            txt.RemoveAt(j);
                            j--;
                        }
                    }
                    csv.Add(id, txt.ToArray());
                }
                return csv;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void WriteFile(string path, string content, bool append = false)
        {
            using (StreamWriter writer = new StreamWriter(path, append, Encoding.UTF8))
            {
                writer.Write(content);
            }
        }
        public static string ReadFile(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                UnityEngine.Debug.LogError(path + ":不存在");
                return "";
            }
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        public static long GetFileLength(string path)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return 0;
            }
            return file.Length;
        }
        public static void CreateDir(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                info.Create();
            }
        }
        public static void Open(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                return;
            }
            Process p = new Process();
#if UNITY_EDITOR_WIN
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start " + path;
#elif UNITY_EDITOR_OSX
            p.StartInfo.FileName = "bash";
            p.StartInfo.Arguments = Application.dataPath + "/MiniFramework/0.Core/Editor/OpenDir.sh" + " " + path;
#endif
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            p.Close();
        }
    }
}