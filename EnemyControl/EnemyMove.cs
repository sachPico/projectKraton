using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform playfieldAnchor;
    public float speed, health, spawnPowerUp;
    //public float difficultyDensityIntensity, difficultySpeedIntensity, bulletDensity;

    /*[System.Serializable]
    public struct Parameters
    {
        public float range, targetDirection, bulletDirection, bulletDensity, bulletSpeed, difficultyDensity, difficultySpeed;
        public bool isAimingPlayer;
    }*/

    public List<BulletGenerator> bg_list=new List<BulletGenerator>();
    Vector3 spawnLocalPosition;
    //Actions actions;
    //public static List<Actions.Action> actionList;
    //public Parameters param;


    void Start()
    {
        spawnLocalPosition=new Vector3(0,0,0);
        //actions = new Actions();
        //bg_list = new List<BulletGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.right*speed*Time.deltaTime;
        //bg_list[0].GeneratorShoot();
        for(int i=0; i<bg_list.Count; i++)
        {
            //Debug.Log("Generator Shoot "+i+" "+bg_list[i].location.x+" "+bg_list[i].location.y);
            bg_list[i].GeneratorShoot();
            //Debug.Log("Generator Shoot "+i+" "+bg_list[i].location.x+" "+bg_list[i].location.y+" "+bg_list[i].location.z);
        }
        /*if(fireRate.timerCount())
        {
            switch(pattern)
            {
                case Pattern.ShootAim: acts._shootAim.Execute
                (
                    this.gameObject,
                    "Kerikil1",
                    param
                );
                break;
                case Pattern.Circular: acts._shootCircular.Execute
                (
                    this.gameObject,
                    "Kerikil1",
                    param
                );
                break;
                case Pattern.ShootFan: acts._shootFan.Execute
                (
                    this.gameObject,
                    "Kerikil1",
                    param
                );
                break;
            }
        }*/
        if(GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).x>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).x<-0.1f||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y<-0.1)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PlayerBullet")
        {
            other.gameObject.SetActive(false);
            health-=1;
            if(health<=0)
            {
                for(int i=0; i<spawnPowerUp; i++)
                {
                    spawnLocalPosition.x=Random.Range(-3f,3f);
                    spawnLocalPosition.y=Random.Range(-3f,3f);
                    GameController.sharedOverseer.spawnPowerUp(transform.localPosition+spawnLocalPosition);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
