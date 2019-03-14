using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFramework
{
    public class UITextComponent : Text
    {
        public int ID;
        protected override void Start()
        {
            base.Start();
            if (Application.isPlaying)
            {
                SwitchText();
                Language.Instance.ChangeLanguageHandler += () =>
                {
                    SwitchText();
                };
            }
        }
        void SwitchText()
        {
            if (Language.Instance.Words.ContainsKey(ID))
            {
                text = Language.Instance.Words[ID];
                text = text.Replace("\\n","\n");
            }
        }
    }
}