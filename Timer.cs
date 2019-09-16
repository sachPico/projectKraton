using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer_i, timer;
    public bool isOver, isResettable;


    public Timer(float t, bool resettability)
    {
        timer_i=1;
        timer = t;
        isOver = false;
        isResettable = resettability;
    }
    public void changeDuration(float duration)
    {
        timer = duration;
    }
    public bool timerCount()
    {
        if(timer_i >= timer)
        {
            timer_i=0;
            isOver=true;
            if(isResettable)
            {
                timer_i=0;
            }
            return true;
        }
        else
        {
            timer_i+=Time.deltaTime;
            return false;
        }
    }
}
