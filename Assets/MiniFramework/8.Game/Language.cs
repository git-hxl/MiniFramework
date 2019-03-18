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
            简体中文,
            繁体中文,
            English,
        }
        public LanguageType ChangeLanguageType;//修改目标语言
        public Action ChangeLanguageEvent;//语言修改事件
        public Dictionary<int, string> CurLanguageWords = new Dictionary<int, string>();//文本
        private Dictionary<int, string[]> allLanguageWords = new Dictionary<int, string[]>();//文本
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
        private void Load()
        {
            FileUtil.ReadFromLocalAsync(Application.streamingAssetsPath + "/Language.csv", (data) =>
            {
                txts = Encoding.UTF8.GetString((byte[])data);
                Read();
            });
        }
        void Update()
        {
            if (curLanguageType != ChangeLanguageType)
            {
                curLanguageType = ChangeLanguageType;
                ChangeLanguage();
            }
        }

        void Read()
        {
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
                        txt[j] = txt[j].Replace("\\n","\n");
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
                    allLanguageWords.Add(id, txt.ToArray());
                }
                foreach (var item in allLanguageWords)
                {
                    CurLanguageWords.Add(item.Key, item.Value[(int)curLanguageType]);
                }
                if (ChangeLanguageEvent != null)
                {
                    ChangeLanguageEvent();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("语言初始化失败：" + e);
            }
        }
        void ChangeLanguage()
        {
            foreach (var item in allLanguageWords)
            {
                CurLanguageWords[item.Key] = item.Value[(int)curLanguageType];
            }
            if (ChangeLanguageEvent != null)
            {
                ChangeLanguageEvent();
            }
        }
    }
}