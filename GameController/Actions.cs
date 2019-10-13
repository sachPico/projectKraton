using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{
    public class Action{
        public virtual void Execute(GameObject client, string tag, Vector3 position, float speed){}
        public virtual void Execute(GameObject client, string tag, Vector3 position, float speed, int intensity, int additionalFactor){}
    }

    

    public class ShootAim:Action
    {
        public override void Execute(GameObject client, string tag, Vector3 position, float speed)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-client.transform.localPosition.y,pLocation.x-client.transform.localPosition.x));
            GameController.sharedOverseer.shoot(tag,position,direction,speed, Color.red);
        }
    }

    public class ShootCircular:Action
    {
        public override void Execute(GameObject client, string tag, Vector3 position, float speed, int intensity, int otherFactor)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-client.transform.localPosition.y,pLocation.x-client.transform.localPosition.x));
            for(int i=0; i<intensity; i++)
            {
                GameController.sharedOverseer.shoot(tag,position,direction+(i*360/intensity)+otherFactor,speed, Color.red);
            }
        }
    }

    public Action _shootAim = new ShootAim();
    public Action _shootCircular = new ShootCircular();
}