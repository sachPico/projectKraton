using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    private float timer_i;
    public float timer;
    public bool isOver, isResettable;


    public Timer(float t, bool setResettable)
    {
        timer_i=0;
        timer = t;
        isResettable = setResettable;
        isOver=false;
    }
    public void changeDuration(float duration)
    {
        timer = duration;
    }

    public void resetTimer()
    {
        isOver=false;
    }

    public bool timerCount()
    {
        if(isOver==false)
        {
            if(timer_i >= timer)
            {
                if(!isResettable)
                {
                    isOver=true;
                }
                timer_i=0;
                return true;
            }
            else
            {
                timer_i+=Time.deltaTime;
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}
