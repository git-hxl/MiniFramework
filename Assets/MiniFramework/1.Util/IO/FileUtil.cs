using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileUtil
{
    public static Dictionary<int, string[]> ReadCSV(string path)
    {
        string txts = File.ReadAllText(path);
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
}
