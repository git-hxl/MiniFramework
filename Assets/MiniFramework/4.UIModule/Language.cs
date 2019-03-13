using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        private LanguageType curLanguageType;
        public LanguageType ChangeLanguageType;
        public Dictionary<int, string> Words = new Dictionary<int, string>();
        public Object[] texts;
        private string txts;
        protected override void OnSingletonInit()
        {
            curLanguageType = ChangeLanguageType;
        }
        void Start()
        {
            texts = FindObjectsOfType(typeof(Text));
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
            string[] txt2Array = txts.Split('\n');
            for (int i = 1; i < txt2Array.Length; i++)
            {
                string[] txt = txt2Array[i].Split(',');
                int id = int.Parse(txt[0]);
                string targetLanguage = txt[(int)curLanguageType + 1];
                if (Words.ContainsKey(id))
                {
                    Words[id] = targetLanguage;
                }
                else
                {
                    Words.Add(id, targetLanguage);
                }

            }
        }

    }
}

