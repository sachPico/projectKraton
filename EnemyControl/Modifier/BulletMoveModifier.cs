using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletMoveModifier
{
    public BulletMovement parentBM;
    public float angleSpeed;

    // Update is called once per frame
    public void RotateDirection()
    {
        if(parentBM!=null)
        {
            parentBM.transform.Rotate(0,0,angleSpeed*Time.deltaTime);
        }
    }
}
