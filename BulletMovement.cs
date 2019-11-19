using System;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    protected float acceleration, maxSpeed, minSpeed;
    public float speed, modifierTargetRotation, interpolationSpeed, initialZ, angleSpeed;
    public BulletMoveModifier bmm;

    void Update()
    {
        transform.position+=transform.right*speed*Time.fixedDeltaTime;
        if(bmm!=null)
        {
            bmm.RotateDirection();
        }
        //transform.Rotate(0,0,angleSpeed*Time.deltaTime);
        if(transform.localPosition.x>-GameController.fieldBorder||transform.localPosition.x<GameController.fieldBorder||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y<-0.1)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy"&&this.tag!="EnemyBullet")
        {
            GameController.sharedOverseer.gameScore.AddValue(10);
            this.gameObject.SetActive(false);
        }
    }

}
