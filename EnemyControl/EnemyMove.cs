using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed, health, spawnPowerUp;
    public List<BulletGenerator> bg_list=new List<BulletGenerator>();
    public List<Timer> generatorTimer = new List<Timer>();
    Vector3 spawnLocalPosition;


    void Start()
    {
        spawnLocalPosition=new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.right*speed*Time.deltaTime;
        
        for(int i=0; i<generatorTimer.Count; i++)
        {
            /*if(checkVisibility.x>1.1||checkVisibility.x<-0.1||checkVisibility.y>1.1||checkVisibility.y<-0.1)
            {
                bg_list[i].isShoot=false;
            }
            else
            {
                bg_list[i].isShoot=true;
            }*/
            if(generatorTimer[0].timerCount())
            {
                if(i!=0&&generatorTimer[i-1].isOver)
                {
                    generatorTimer[i].timerCount();
                }
            }
            else
            {
                break;
            }
        }
        if(transform.localPosition.x>-GameController.fieldBorder||transform.localPosition.x<GameController.fieldBorder||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y>1.3||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y<-0.1)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PlayerBullet")
        {
            //other.gameObject.SetActive(false);
            health-=1;
            if(health<=0)
            {
                for(int i=0; i<spawnPowerUp; i++)
                {
                    spawnLocalPosition.x=Random.Range(-3f,3f);
                    spawnLocalPosition.y=Random.Range(-3f,3f);
                    GameController.sharedOverseer.spawnPowerUp(transform.localPosition+spawnLocalPosition);
                }
                GameController.sharedOverseer.gameScore.AddValue(100);
                gameObject.SetActive(false);
            }
        }
    }
}
