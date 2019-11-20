using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Score
{
    public Value _value;
    public int max;

    public Score(int scoreIndex)
    {
        max = 0;
        _value = new Value(scoreIndex);
    }

    public Score(int maxValue, int scoreIndex)
    {
        max = maxValue;
        _value = new Value(scoreIndex);
    }

    public void AddValue(uint addValue)
    {
        _value.value += addValue;
        if(max!=0)
        {
            _value.value = (uint)Mathf.Clamp(_value.value, -1, max);
        }
        ScoreUIHandler.scoreHandler.UpdateScore(_value);
    }
}