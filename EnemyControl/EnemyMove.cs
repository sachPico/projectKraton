using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform playfieldAnchor;
    public float speed, health, spawnPowerUp, bulletSpeed;
    public float difficultyDensityIntensity, difficultySpeedIntensity, bulletDensity;
    Vector3 spawnLocalPosition;
    Timer fireRate;
    //Actions actions;
    //public static List<Actions.Action> actionList;
    public enum Pattern{ShootAim, Circular};
    public Pattern pattern;
    public Actions acts;


    void Start()
    {
        fireRate = new Timer(1f,true);
        spawnLocalPosition=new Vector3(0,0,0);
        //actions = new Actions();
        acts = new Actions();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.right*speed*Time.deltaTime;
        if(fireRate.timerCount())
        {
            switch(pattern)
            {
                case Pattern.ShootAim: acts._shootAim.Execute
                (
                    this.gameObject,
                    "Kerikil1",
                    transform.localPosition,
                    bulletSpeed*difficultySpeedIntensity*(int)GameController.sharedOverseer.difficulty
                );
                break;
                case Pattern.Circular: acts._shootCircular.Execute
                (
                    this.gameObject,
                    "Kerikil1",
                    transform.localPosition,
                    bulletSpeed*difficultySpeedIntensity*(int)GameController.sharedOverseer.difficulty,
                    (int)(bulletDensity*difficultyDensityIntensity*(int)GameController.sharedOverseer.difficulty),
                    0
                );
                break;
            }
        }
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
