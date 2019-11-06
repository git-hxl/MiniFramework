using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class SuperInputText : InputField
{
    List<string> patterns = new List<string>();
    protected override void Awake()
    {
        base.Awake();
        patterns.Add(@"\p{Cs}");
        patterns.Add(@"[\u2702-\u27B0]");
        onValidateInput = MyOnValidateInput;
    }
    private char MyOnValidateInput(string text, int charIndex, char addedChar)
    {
        if (patterns.Count > 0)
        {
            string s = string.Format("{0}", addedChar);
            if (BEmoji(s))
            {
                return '\0';
            }
        }
        return addedChar;
    }
    private bool BEmoji(string s)
    {
        bool bEmoji = false;
        for (int i = 0; i < patterns.Count; ++i)
        {
            bEmoji = Regex.IsMatch(s, patterns[i]);
            if (bEmoji)
            {
                break;
            }
        }
        return bEmoji;
    }
    public void AddPatterns(string s)
    {
        patterns.Add(s);
    }
    public void ClearPatterns(string s)
    {
        patterns.Clear();
    }
}