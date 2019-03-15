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
                Language.Instance.ChangeLanguageEvent += () =>
                {
                    SwitchText();
                };
            }
        }
        void SwitchText()
        {
            if (Language.Instance.CurLanguageWords.ContainsKey(ID))
            {
                text = Language.Instance.CurLanguageWords[ID];
            }
        }
    }
}