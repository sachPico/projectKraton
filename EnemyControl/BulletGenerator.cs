using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletGenerator:MonoBehaviour
{
    public Vector3 firstLoc;
    Vector3 checkVisibility;
    //Variables that must be generated in GameController
    public Color bulletColor;
    public BulletParameter parameter;
    public Actions.Action act;
    public bool isShoot = false, isGatling;
    //[ConditionalHide("isGatling", true)]
    public Timer gatlingTimer;

    void Update()
    {
        if(gatlingTimer!=null&&!gatlingTimer.isOver)
        {
            gatlingTimer.timerCount();
        }
    }

    public void GeneratorShoot()
    {
        act.Execute(transform.localPosition+transform.parent.localPosition, parameter, bulletColor);
        
        //Debug.Log(firstLoc.x+" "+firstLoc.y+" "+firstLoc.z);
    }
    
}