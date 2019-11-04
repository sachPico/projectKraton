using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    private float timer_i;
    int iterations;
    public int maxIterations;
    public float timer;
    public bool isOver, isResettable;

    public delegate void ExecuteDelegate();
    public event ExecuteDelegate ExecuteEvent;

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
            //Time is up
            if(timer_i >= timer)
            { 
                timer_i=0;
                
                if(ExecuteEvent!=null)
                {
                    ExecuteEvent();
                }
                if(isResettable&&iterations<maxIterations)
                {
                    iterations++;
                }
                else
                {
                    isOver=true;
                }
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
