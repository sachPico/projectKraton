using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions:MonoBehaviour
{
    
    public class Action{
        protected GameObject fired;
        protected BulletMovement bm;
        public virtual void Execute(Vector2 spawnPosition, BulletParameter parameter, Color color){}
    
    }

    void CalculateDensity()
    {
        
    }

    public class ShootLayered:Action
    {
        public override void Execute(Vector2 spawnPos, BulletParameter param, Color col)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            int finalDensity = (int)(param.bulletDensity*param.difficultyDensity*(int)GameController.sharedOverseer.difficulty);
            float densitySpeed=(param.bulletSpeed-5)/(finalDensity-1);
            if(param.isAimingPlayer)
            {
                float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-spawnPos.y,pLocation.x-spawnPos.x));
                for(int i=0; i<finalDensity; i++)
                {
                    fired = GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,direction,5+(i*densitySpeed), col);
                    if(param.isModifyRotation)
                    {
                        bm = fired.GetComponent<BulletMovement>();
                        bm.bmm = new BulletMoveModifier();
                        bm.bmm.parentBM = bm;
                        bm.bmm.angleSpeed=param.angleSpeedModifier;
                    }
                }
            }
            else
            {
                for(int i=0; i<finalDensity; i++)
                {
                    GameObject fired = GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,param.targetDirection,5+(densitySpeed*i), col);
                    if(param.isModifyRotation)
                    {
                        bm = fired.GetComponent<BulletMovement>();
                        bm.bmm = new BulletMoveModifier();
                        bm.bmm.parentBM = bm;
                        bm.bmm.angleSpeed=param.angleSpeedModifier;
                    }
                }
            }
        }
    }

    public class ShootDirectional:Action
    {
        public override void Execute(Vector2 spawnPos, BulletParameter param, Color col)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            if(param.isAimingPlayer)
            {
                float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-spawnPos.y,pLocation.x-spawnPos.x));
                fired = GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,direction,param.bulletSpeed, col);
                if(param.isModifyRotation)
                    {
                        fired.AddComponent(typeof(BulletMoveModifier));
                        fired.GetComponent<BulletMoveModifier>().angleSpeed=param.angleSpeedModifier;
                    }
            }
            else
            {
                fired = GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,param.targetDirection,param.bulletSpeed, col);
                if(param.isModifyRotation)
                    {
                        fired.AddComponent(typeof(BulletMoveModifier));
                        fired.GetComponent<BulletMoveModifier>().angleSpeed=param.angleSpeedModifier;
                    }
            }
            
        }
    }

    public class ShootCircular:Action
    {

        public override void Execute(Vector2 spawnPos, BulletParameter param, Color col)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            float direction = Mathf.Rad2Deg*(Mathf.Atan2(pLocation.y-spawnPos.y,pLocation.x-spawnPos.x));
            int finalDensity = (int)(param.bulletDensity*param.difficultyDensity*(int)GameController.sharedOverseer.difficulty);
            for(int i=0; i<finalDensity; i++)
            {
                if(param.isAimingPlayer)
                {
                    fired = GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,direction+(i*360/finalDensity),param.bulletSpeed, col);
                    if(param.isModifyRotation)
                    {
                        fired.AddComponent(typeof(BulletMoveModifier));
                        fired.GetComponent<BulletMoveModifier>().angleSpeed=param.angleSpeedModifier;
                    }
                }
                else
                {
                    fired = GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,param.targetDirection+(i*360/finalDensity),param.bulletSpeed, col);
                    if(param.isModifyRotation)
                    {
                        fired.AddComponent(typeof(BulletMoveModifier));
                        fired.GetComponent<BulletMoveModifier>().angleSpeed=param.angleSpeedModifier;
                    }
                }
                
            }
        }
    }

    public class ShootFan:Action
    {
        public override void Execute(Vector2 spawnPos, BulletParameter param, Color col)
        {
            Vector3 pLocation = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            int finalDensity = (int)(param.bulletDensity*param.difficultyDensity*(int)GameController.sharedOverseer.difficulty);
            float lowerDirection;
            float steps = (param.range*2)/(finalDensity-1==0?finalDensity=1:finalDensity-1);
            if(param.isAimingPlayer)
            {
                float direction = Mathf.Rad2Deg*(Mathf.Atan2(
                pLocation.y-spawnPos.y,
                pLocation.x-spawnPos.x));
                lowerDirection = (direction - param.range)%360;
            }
            else
            {
                lowerDirection = (param.targetDirection - param.range);
            }
            for(int i=0; i<finalDensity; i++)
            {
                GameController.sharedOverseer.shoot(param.bulletLabel,spawnPos,lowerDirection+(i*steps),param.bulletSpeed, col);
            }
        }
    }

    public static Action _shootDirection = new ShootDirectional();
    public static Action _shootCircular = new ShootCircular();
    public static Action _shootFan = new ShootFan();
    public static Action _shootLayered = new ShootLayered();
}