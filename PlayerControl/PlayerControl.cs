using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    int powerStatus;
    public int powerCounter;
    public float dist;
    private Vector3 limit, reset;
    public Vector3 translate;
    bool isShoot;

    public List<PlayerBulletSpawn> pBulletSpawn = new List<PlayerBulletSpawn>();

    public delegate void ShootDelegate();
    public static event ShootDelegate shootEvent;

    public class PlayerBulletSpawn
    {
        static Transform playerPos;
        Vector3 dist;
        public float fireDirection, fireSpeed;
        public string bulletLabel;
        public PlayerBulletSpawn(string bl, float d, float r, float fd, float fs)
        {
            bulletLabel = bl;
            dist.x = Mathf.Cos(Mathf.Deg2Rad*d)*r;
            dist.y = Mathf.Sin(Mathf.Deg2Rad*d)*r;
            fireDirection = fd;
            fireSpeed = fs;
            if(playerPos==null)
                playerPos = GameController.sharedOverseer.player.transform;
        }
        public void OnShoot()
        {
            GameController.sharedOverseer.shoot(bulletLabel, playerPos.localPosition+dist, fireDirection,fireSpeed, Color.white);
        }
        public void OnReduceGenerator()
        {
            PlayerControl.shootEvent-=OnShoot;
        }
    }

    void Start()
    {
        isShoot=false;
        translate = new Vector3(0,0,0);
        reset=new Vector3(0,-3,0);

        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 80f, .9f, 90f, 100f));
        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 100f, .9f, 90f, 100f));

        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 75f, .9f, 75f, 100f));
        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 105f, .9f, 105f, 100f));

        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 60f, .9f, 60f, 100f));
        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 120f, .9f, 120f, 100f));

        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 330f, .5f, 330f, 100f));
        pBulletSpawn.Add(new PlayerBulletSpawn("AlifNormal", 210f, .5f, 210f, 100f));

        for(int i=0; i<2; i++)
        {
            shootEvent += pBulletSpawn[i].OnShoot;
        }
    }

    void PowerUp()
    {
        
        powerStatus++;
        Debug.Log(powerStatus);
        shootEvent+=pBulletSpawn[(powerStatus*2)+1].OnShoot;
        shootEvent+=pBulletSpawn[powerStatus*2].OnShoot;
        powerCounter=0;
    }

    void PowerDown()
    {
        shootEvent-=pBulletSpawn[(powerStatus*2)+1].OnShoot;
        shootEvent-=pBulletSpawn[powerStatus*2].OnShoot;
        powerStatus--;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            if(isShoot==false)
            {
                isShoot=true;
            }
            else
            {
                //GameController.sharedOverseer.shoot("AlifNormal",transform.localPosition+(Vector3.right*dist), 90f,100f, Color.white);
                //GameController.sharedOverseer.shoot("AlifNormal",transform.localPosition+(Vector3.right*dist*-1), 90f,100f, Color.white);
                shootEvent();
                isShoot=false;
            }
        }
        translate.x=Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime;
        translate.y=Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime;
        transform.localPosition+= translate;
        
    }

    void LateUpdate()
    {
        limit = transform.localPosition;
        limit.x = Mathf.Clamp(limit.x,GameController.sharedOverseer.border_test[0].localPosition.x, GameController.sharedOverseer.border_test[1].localPosition.x);
        limit.y = Mathf.Clamp(limit.y,GameController.sharedOverseer.border_test[2].localPosition.y, GameController.sharedOverseer.border_test[3].localPosition.y);

        transform.localPosition = limit;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="EnemyBullet")
        {
            other.gameObject.SetActive(false);
            GameController.sharedOverseer.PlayerDeath();
            if(powerStatus!=0)
            {
                PowerDown();
            }
            if(GameController.sharedOverseer.playerLifes!=0)
            {
                transform.localPosition=reset;
            }
        }
        if(other.tag=="Item")
        {
            if(powerStatus!=1)
            {
                powerCounter++;
            }
            if(powerCounter>=5)
            {
                PowerUp();
            }
        }
    }
}
