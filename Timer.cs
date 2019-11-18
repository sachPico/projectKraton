using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    private float timer_i;
    public int iterations=0;
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
        iterations=0;
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
                
                
                if(isResettable)
                {
                    if(iterations<maxIterations)
                    {
                        iterations++;
                        ExecuteEvent();
                    }
                    else
                    {
                            isOver=true;
                    }
                }
                
                else
                {
                    isOver=true;
                    if(ExecuteEvent!=null)
                    {
                        ExecuteEvent();
                    }
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
