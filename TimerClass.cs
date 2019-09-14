using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerClass
{
    private int timer_i, timer;
    public bool isOver;
    public TimerClass(int t)
    {
        timer_i=1;
        timer = t;
        isOver = false;
    }
    public void timerCount()
    {
        if(timer_i >= timer)
        {
            timer_i=0;
            isOver=true;
        }
        else
        {
            timer_i++;
        }
    }
}
