using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    int powerStatus;
    public int powerCounter;
    public float dist;
    bool enableRenderer = true, enableCollider = true;
    Timer onRevivalTimer;
    SphereCollider pCollider;
    MeshRenderer pMeshRenderer;
    private Vector3 limit, reset;
    public Vector3 translate;
    bool isShoot, isThreadAlreadyInitiated=false;

    public List<PlayerBulletSpawn> pBulletSpawn = new List<PlayerBulletSpawn>();

    Thread onRevivalThread;

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

        pCollider = this.gameObject.GetComponent<SphereCollider>();
        pMeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        onRevivalTimer = new Timer(3f, false);
        onRevivalThread = new Thread(OnRevival);

        for(int i=0; i<2; i++)
        {
            shootEvent += pBulletSpawn[i].OnShoot;
        }
    }

    void Print()
    {
        onRevivalThread.Interrupt();
        onRevivalThread=null;
        if(onRevivalThread==null)
        {
            Debug.Log("HOYAA ");
            isThreadAlreadyInitiated=true;
        }
        
    }

    void Print2()
    {
        Debug.Log("HAE");
    }

    bool TimerEncapsulator(Timer inputTimer)
    {
        return inputTimer.timerCount();
    }

    void OnRevival()
    {
        for(int i=0; i<1000; i++)
        {
            Thread.Sleep(4);
            enableRenderer=!enableRenderer;
        }
        
        enableRenderer=true;
        enableCollider=true;
        Debug.Log(enableCollider);
        Print();
    }

    void OnEnable()
    {
        if(onRevivalThread!=null)
        {
            onRevivalThread.Start();
        }
        else if(onRevivalThread==null && isThreadAlreadyInitiated==true)
        {
            onRevivalThread=new Thread(OnRevival);
            onRevivalThread.Start();
        }
    }

    void PowerUp()
    {
        
        powerStatus++;
        //Debug.Log(powerStatus);
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
        pMeshRenderer.enabled = enableRenderer;
        pCollider.enabled = enableCollider;
        
        if(Input.GetButton("Fire1"))
        {
            if(isShoot==false)
            {
                isShoot=true;
            }
            else
            {
                shootEvent();
                isShoot=false;
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            PowerUp();
        }
        translate.x=(Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime)*(Input.GetAxisRaw("Focus")==1?.5f:1f);
        translate.y=(Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime)*(Input.GetAxisRaw("Focus")==1?.5f:1f);
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
            if(GameController.sharedOverseer.playerLifes>=0)
            {
                enableCollider=false;
                this.gameObject.SetActive(false);
            }
        }
        if(other.tag=="Item")
        {
            if(powerStatus!=8)
            {
                powerCounter++;
                //Debug.Log(powerCounter);
            }
            if(powerCounter>=100)
            {
                PowerUp();
            }
        }
    }
}
