using System;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    protected float acceleration, maxSpeed, minSpeed;
    public float speed, modifierTargetRotation, interpolationSpeed, initialZ;
    public bool isExecuteModifier;
    Modifier modifier;
    Timer timer;

    ///<summary>
    ///Give an object a speed by s.
    ///Set accel to 0 if you don't want to apply any acceleration.
    ///</summary>
    public BulletMovement(float s, float accel)
    {
        speed = s;
        acceleration = accel;
    }
    ///<summary>
    ///Give an object a speed by s with maximum speed equals to maxS.
    ///Set accel to 0 if you don't want to apply any acceleration.
    ///</summary>
    public BulletMovement(float s, float maxS, float accel)
    {
        speed = s;
        maxSpeed = maxS;
        acceleration = accel;
    }
    ///<summary>
    ///Give an object a speed by s that must be between minS and maxS.
    ///Set accel to 0 if you do't want to apply any acceleration. 
    ///</summary>
    public BulletMovement(float s, float maxS, float minS, float accel)
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

    void Start()
    {
        modifier = new Modifier();
        timer = new Timer(.2f,false);
    }

    void Update()
    {
        transform.position+=transform.right*speed*Time.deltaTime;
        if(GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).x>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).x<-0.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y<-0.1)
        {
            timer.resetTimer();
            gameObject.SetActive(false);
        }
        if(timer.timerCount()&&isExecuteModifier)
        {
            modifier._changeRotationClamped.Modify(this.transform, initialZ+modifierTargetRotation, interpolationSpeed);
        }
    }
}
