using System;
using UnityEngine;

public class SpeedClass : MonoBehaviour
{
    protected float acceleration, maxSpeed, minSpeed;
    public float speed;

    ///<summary>
    ///Give an object a speed by s.
    ///Set accel to 0 if you don't want to apply any acceleration.
    ///</summary>
    public SpeedClass(float s, float accel)
    {
        speed = s;
        acceleration = accel;
    }
    ///<summary>
    ///Give an object a speed by s with maximum speed equals to maxS.
    ///Set accel to 0 if you don't want to apply any acceleration.
    ///</summary>
    public SpeedClass(float s, float maxS, float accel)
    {
        speed = s;
        maxSpeed = maxS;
        acceleration = accel;
    }
    ///<summary>
    ///Give an object a speed by s that must be between minS and maxS.
    ///Set accel to 0 if you do't want to apply any acceleration. 
    ///</summary>
    public SpeedClass(float s, float maxS, float minS, float accel)
    {
        speed = s;
        maxSpeed = maxS;
        minSpeed = minS;
        acceleration = accel;
    }

    private void clampSpeed()
    {
        speed = speed < minSpeed ? minSpeed : speed > maxSpeed ? maxSpeed : speed;
    }
    public void accelerate()
    {
        speed += acceleration;
        clampSpeed();
    }
    public void decelerate()
    {
        speed -= acceleration;
        clampSpeed();
    }

    void Update()
    {
        transform.localPosition+=new Vector3(transform.right.x*speed*Time.deltaTime, transform.right.y*speed*Time.deltaTime);
    }
}
