using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Score
{
    public string tag;
    public Value _value=new Value();
    public int max;

    public Score(string scoreTag)
    {
        max = 0;
        tag = scoreTag;
    }

    public Score(int maxValue, string scoreTag)
    {
        max = maxValue;
        tag = scoreTag;
    }

    public void AddValue(uint addValue)
    {
        _value.value += addValue;
        if(max!=0)
        {
            _value.value = (uint)Mathf.Clamp(_value.value, 0, max==0? int.MaxValue : max);
        }
        ScoreUIHandler.scoreHandler.UpdateScore(_value);
    }
}