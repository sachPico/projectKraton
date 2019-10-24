using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{

    public class Action{
        /*public virtual void Execute(GameObject client, string tag, Vector3 position, float speed){}
        public virtual void Execute(GameObject client, string tag, Vector3 position, float speed, int intensity, int additionalFactor){}
        public virtual void Execute(GameObject client, string tag, Vector3 position, float direction, float range, float speed, int intensity, int additionalFactor){}
        public virtual void Execute(GameObject client, string tag, Vector3 position, EnemyMove.Parameters parameter){}*/
        public virtual void Execute(GameObject client, string tag, float speed){}
        public virtual void Execute(GameObject client, string tag, float speed, int intensity, int additionalFactor){}
        public virtual void Execute(GameObject client, string tag, float direction, float range, float speed, int intensity, int additionalFactor){}
        public virtual void Execute(GameObject client, string tag, BulletParameter parameter){}
        public virtual void Execute(Vector2 spawnPosition, BulletParameter parameter){}
        
    }

    void CalculateDensity()
    {
        
    }

    public class ShootAim:Action
    {
        public override void Execute(Vector2 spawnPos, BulletParameter param)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-spawnPos.y,pLocation.x-spawnPos.x));
            GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,direction,param.bulletSpeed, Color.red);
        }
    }

    public class ShootCircular:Action
    {

        public override void Execute(Vector2 spawnPos, BulletParameter param)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-spawnPos.y,pLocation.x-spawnPos.x));
            for(int i=0; i<param.bulletDensity; i++)
            {
                GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,param.bulletDirection+(i*360/param.bulletDensity),param.bulletSpeed, Color.red);
            }
        }
    }

    public class ShootFan:Action
    {
        public override void Execute(Vector2 spawnPos, BulletParameter param)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            int finalDensity = (int)(param.bulletDensity*param.difficultyDensity*(int)GameController.sharedOverseer.difficulty);
            float lowerDirection;
            float steps = (param.range*2)/(finalDensity-1);
            if(param.isAimingPlayer)
            {
                float direction = Mathf.Rad2Deg*(Mathf.Atan2(
                pLocation.y-spawnPos.y,
                pLocation.x-spawnPos.x));
                lowerDirection = direction - param.range;
            }
            else
            {
                lowerDirection = (param.targetDirection - param.range);
            }
            for(int i=0; i<finalDensity; i++)
            {
                GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,lowerDirection+(i*steps),param.bulletSpeed, Color.red);
            }
        }
    }

    public static Action _shootAim = new ShootAim();
    public static Action _shootCircular = new ShootCircular();
    public static Action _shootFan = new ShootFan();
}