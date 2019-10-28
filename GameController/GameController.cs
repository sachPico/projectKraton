using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }
    [System.Serializable]
    public class SpawnObject
    {
        public Timer timer;
        public SpawnObjectProperty[] enemySpawn;
        
        public void Generate()
        {
            for(int m=0; m<enemySpawn.Length; m++)
            {
                GameController.sharedOverseer.generateEnemy(enemySpawn[m]);
            }
        }
    }
    [System.Serializable]
    public class SpawnObjectProperty{
        public GameObject obj;
        public Vector2 camPosition;
        public BulletGeneratorProperty[] bulletGenerator;
        public float direction, speed, spawnPowerUpNumber, health;
    }

    [System.Serializable]
    public class BulletGeneratorProperty{
        public float direction, radius;
        public List<Timer> fireTime;
        public BulletPatternProperty[] pattern;
    }

    [System.Serializable]
    public class BulletPatternProperty
    {
        public BulletParameterProperty[] parameter;
        public enum PatternAction {ShootAim, ShootFan, ShootLayered, Circular};
        public PatternAction[] actions;
    }

    [System.Serializable]
    public class BulletParameterProperty
    {
        public float range, targetDirection, bulletDirection, bulletDensity, bulletSpeed, difficultyDensity, difficultySpeed, angleSpeedModifier;
        public bool isAimingPlayer, isModifyRotation;
        public string bulletLabel;
    }

    public enum Difficulty{Easy=1, Normal, Hard};
    public enum Act{ShootAim, Circular, ShootFan};
    public Difficulty difficulty;

    public Dictionary<string, List<GameObject>> bulletDictionary;
    public List<Pool> bulletPools;
    public SpawnObject[] spawnObjects;
    BulletGenerator bake_bulletGenerator = new BulletGenerator();
    BulletPattern bake_bulletPattern = new BulletPattern();
    BulletParameter bake_bulletParameter = new BulletParameter();
    Vector3 bakePos=new Vector3();
    public Vector3[] border_cam, border;
    public static double fieldBorder;

    public GameObject enemies, player;
    public Camera mainCam;
    public Transform playfieldAnchorTransform;
    public Transform[] border_test;
    public static GameController sharedOverseer;

    public int playerLifes=2;

    //mending ga usah dijadiin satu class sendiri
    #region EnemyGenerationPattern

    private Transform camTransform;
    private int spawnCounter=0;//, j=0;
    private Timer generateTimer;

    public void generateEnemy(SpawnObjectProperty property)
    {
        Vector3 toAnchor = mainCam.ViewportToWorldPoint(new Vector3(property.camPosition.x,property.camPosition.y,Mathf.Abs(mainCam.transform.localPosition.z)));
        GameObject enemy = Instantiate(property.obj, toAnchor, Quaternion.identity,playfieldAnchorTransform);
        enemy.transform.localRotation = Quaternion.Euler(0,0,property.direction);
        EnemyMove em = enemy.GetComponent<EnemyMove>();
        em.health                   = property.health;
        em.spawnPowerUp             = property.spawnPowerUpNumber;
        em.speed                    = property.speed;
        for(int i=0; i<property.bulletGenerator.Length;i++)
        {
            bakePos=new Vector3();
            bake_bulletGenerator=new BulletGenerator();
            for(int j=0; j<property.bulletGenerator[i].pattern.Length;j++)
            {
                bake_bulletPattern=new BulletPattern();
                for(int k=0; k<property.bulletGenerator[i].pattern[j].parameter.Length;k++)
                {
                    bake_bulletParameter=new BulletParameter();
                    bake_bulletParameter.range = property.bulletGenerator[i].pattern[j].parameter[k].range;
                    bake_bulletParameter.bulletDensity = property.bulletGenerator[i].pattern[j].parameter[k].bulletDensity;
                    bake_bulletParameter.bulletDirection = property.bulletGenerator[i].pattern[j].parameter[k].bulletDirection;
                    bake_bulletParameter.bulletSpeed = property.bulletGenerator[i].pattern[j].parameter[k].bulletSpeed;
                    bake_bulletParameter.bulletLabel = property.bulletGenerator[i].pattern[j].parameter[k].bulletLabel;
                    bake_bulletParameter.difficultyDensity = property.bulletGenerator[i].pattern[j].parameter[k].difficultyDensity;
                    bake_bulletParameter.difficultySpeed = property.bulletGenerator[i].pattern[j].parameter[k].difficultySpeed;
                    bake_bulletParameter.targetDirection = property.bulletGenerator[i].pattern[j].parameter[k].targetDirection;
                    bake_bulletParameter.isAimingPlayer = property.bulletGenerator[i].pattern[j].parameter[k].isAimingPlayer;
                    bake_bulletParameter.isModifyRotation = property.bulletGenerator[i].pattern[j].parameter[k].isModifyRotation;
                    bake_bulletParameter.angleSpeedModifier = property.bulletGenerator[i].pattern[j].parameter[k].angleSpeedModifier;
                    //em.bg_list[i].pattern[j].parameter.Add(bake_bulletParameter);
                    //Debug.Log(em.bg_list[i].pattern[j].parameter[k].bulletLabel);
                    switch(property.bulletGenerator[i].pattern[j].actions[k])
                    {
                        case BulletPatternProperty.PatternAction.Circular: bake_bulletPattern.act.Add(Actions._shootCircular); break;
                        case BulletPatternProperty.PatternAction.ShootAim: bake_bulletPattern.act.Add(Actions._shootDirection); break;
                        case BulletPatternProperty.PatternAction.ShootFan: bake_bulletPattern.act.Add(Actions._shootFan); break;
                        case BulletPatternProperty.PatternAction.ShootLayered: bake_bulletPattern.act.Add(Actions._shootLayered); break;
                    }
                    bake_bulletPattern.parameter.Add(bake_bulletParameter);
                }
                bake_bulletGenerator.pattern.Add(bake_bulletPattern);
            }
            bake_bulletGenerator.fireTime=property.bulletGenerator[i].fireTime;
            bakePos.x=Mathf.Cos(property.bulletGenerator[i].direction*Mathf.Deg2Rad)*property.bulletGenerator[i].radius;
            bakePos.y=Mathf.Sin(property.bulletGenerator[i].direction*Mathf.Deg2Rad)*property.bulletGenerator[i].radius;
            bake_bulletGenerator.location=bakePos;
            bake_bulletGenerator.firstLoc=bakePos;
            bake_bulletGenerator.parent=enemy.transform;
            em.bg_list.Add(bake_bulletGenerator);
            em.bg_list[i].isShoot=true;
        }
    }

    public void spawnPowerUp(Vector3 spawnPosition)
    {
        for(int b=0;b<bulletDictionary["PowerUp"].Count;b++)
        {
            if(!bulletDictionary["PowerUp"][b].activeInHierarchy)
            {
                bulletDictionary["PowerUp"][b].SetActive(true);
                bulletDictionary["PowerUp"][b].transform.localRotation=Quaternion.identity;
                bulletDictionary["PowerUp"][b].transform.localPosition=spawnPosition;
                return;
            }
        }
        GameObject obj = Instantiate(bulletDictionary["PowerUp"][0]);
        obj.transform.SetParent(playfieldAnchorTransform);
        obj.transform.localPosition=spawnPosition;
        obj.transform.localRotation=Quaternion.identity;
        obj.SetActive(true);
        bulletDictionary["PowerUp"].Add(obj);
        return;
    }

    public GameObject shoot(string tag, Vector3 position, float rotate, float speed, Color col)
    {
        for(int i=0;i<bulletDictionary[tag].Count;i++)
        {
            if(!bulletDictionary[tag][i].activeInHierarchy)
            {
                bulletDictionary[tag][i].SetActive(true);
                bulletDictionary[tag][i].transform.localPosition=position;
                bulletDictionary[tag][i].transform.localRotation = Quaternion.identity;
                bulletDictionary[tag][i].transform.localRotation = Quaternion.Euler(0,0,rotate);
                BulletMovement spd = bulletDictionary[tag][i].GetComponent<BulletMovement>();
                spd.initialZ = rotate;
                //SpriteRenderer sr = bulletDictionary[tag][i].GetComponent<SpriteRenderer>();
                //sr.color = col;
                spd.speed = speed;
                return bulletDictionary[tag][i];
            }
        }
        GameObject obj = Instantiate(bulletDictionary[tag][0]);
        //SpriteRenderer newsr = obj.GetComponent<SpriteRenderer>();
        //newsr.color = col;
        obj.transform.SetParent(playfieldAnchorTransform);
        obj.transform.localPosition=position;
        obj.transform.localRotation=Quaternion.identity;
        obj.transform.localRotation=Quaternion.Euler(0,0,rotate);
        BulletMovement newspd = obj.GetComponent<BulletMovement>();
        newspd.speed=speed;
        newspd.initialZ=rotate;
        obj.SetActive(true);
        bulletDictionary[tag].Add(obj);
        return obj;
    }

    #endregion

    public void PlayerDeath()
    {
        playerLifes--;
        
    }

    void Awake()
    {
        sharedOverseer = this; 
        Application.targetFrameRate=60;
    }

    void Start()
    {
        
        bulletDictionary=new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in bulletPools)
        {
            List<GameObject> objPool = new List<GameObject>();
            for(int i=0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(playfieldAnchorTransform);
                objPool.Add(obj);
            }
            bulletDictionary.Add(pool.tag, objPool);
        }
        //Horizontal side border
        border_cam[0] = new Vector3(0.1f, 0.5f, transform.localPosition.z);
        border_cam[1] = new Vector3(.9f, 0.5f, transform.localPosition.z);
        //Vertical side border
        border_cam[2] = new Vector3(0.5f, 0.1f, transform.localPosition.z);
        border_cam[3] = new Vector3(0.5f, 0.9f, transform.localPosition.z);
        border[0]=new Vector3();
        border[1]=new Vector3();
        border[2]=new Vector3();
        border[3]=new Vector3();
        for(int i=0; i<4; i++)
        {
            border[i]=new Vector3();
        }
        camTransform=mainCam.transform;
    }

    void Update()
    {
        if(spawnCounter<spawnObjects.Length)
        {
            if(spawnObjects[spawnCounter].timer.timerCount())
            {
                spawnObjects[spawnCounter].Generate();
                spawnCounter++;
            }
        }
        for(int j=0;j<4;j++)
        {
            border_cam[j].z=Mathf.Abs(mainCam.transform.localPosition.z);
            border[j]=mainCam.ViewportToWorldPoint(border_cam[j]);
            border_test[j].position=border[j];
        }
        fieldBorder=(double)(-19.4992f+((camTransform.localPosition.z-20)/10)*4.875f)-1d;
    }
}
