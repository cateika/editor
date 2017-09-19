using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer
{
    public string ans;
    public AnswerLang lang;

    public Answer(string ans, AnswerLang lang)
    {
        this.ans = ans;
        this.lang = lang;
    }
    
}

