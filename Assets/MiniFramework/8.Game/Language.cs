using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
namespace MiniFramework
{
    public class Language : MonoSingleton<Language>
    {
        public enum LanguageType
        {
            China,
            English,
        }
        public LanguageType ChangeLanguageType;//修改目标语言
        public Action ChangeLanguageHandler;//语言修改事件
        public Dictionary<int, string> Words = new Dictionary<int, string>();//文本
        private LanguageType curLanguageType;
        private string txts;
        protected override void OnSingletonInit()
        {
            curLanguageType = ChangeLanguageType;
        }
        void Start()
        {
            Load();
        }
        public void Load()
        {
            FileUtil.ReadFromLocalAsync(Application.streamingAssetsPath + "/Language.csv", (data) =>
            {
                txts = Encoding.UTF8.GetString((byte[])data);
                ExtractWord();
            });
        }
        void Update()
        {
            if (curLanguageType != ChangeLanguageType)
            {
                curLanguageType = ChangeLanguageType;
                ExtractWord();
            }
        }
        void ExtractWord()
        {
            try
            {
                string[] txt2Array = txts.Split('\n');
                for (int i = 1; i < txt2Array.Length; i++)
                {
                    List<string> txt = txt2Array[i].Split(',').ToList();
                    int head = 0;
                    for (int j = 0; j < txt.Count; j++)
                    {
                        if (txt[j].IndexOf("\"") == 0)
                        {
                            head = j;
                            continue;
                        }
                        if (txt[j].IndexOf("\"") == txt[j].Length - 1)
                        {
                            txt[head] = txt[head] + "," + txt[j];
                            txt.RemoveAt(j);
                        }
                    }
                    int id = int.Parse(txt[0]);
                    string targetLanguage = txt[(int)curLanguageType + 1].Trim('"');

                    if (Words.ContainsKey(id))
                    {
                        Words[id] = targetLanguage;
                    }
                    else
                    {
                        Words.Add(id, targetLanguage);
                    }
                }
                if (ChangeLanguageHandler != null)
                {
                    ChangeLanguageHandler();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("语言初始化失败：" + e);
            }
        }
    }
}