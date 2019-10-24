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
        public float generatorDirection, radius;
        public Timer[] fireTime;
        public BulletPatternProperty[] pattern;
    }

    [System.Serializable]
    public class BulletPatternProperty
    {
        public BulletParameterProperty[] parameter;
        public enum PatternAction {ShootAim, ShootFan, Circular};
        public PatternAction[] actions;
    }

    [System.Serializable]
    public class BulletParameterProperty
    {
        public float range, targetDirection, bulletDirection, bulletDensity, bulletSpeed, difficultyDensity, difficultySpeed;
        public bool isAimingPlayer;
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

    public GameObject enemies, player;
    public Camera mainCam;
    public Transform playfieldAnchorTransform;
    public Transform[] border_test;
    public static GameController sharedOverseer;

    //mending ga usah dijadiin satu class sendiri
    #region EnemyGenerationPattern

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
        //int p=0;
        for(int i=0; i<property.bulletGenerator.Length;i++)
        {
            em.bg_list.Add(bake_bulletGenerator);
            em.bg_list[i].parent = enemy.transform;
            bakePos.x = Mathf.Cos(Mathf.Deg2Rad*property.bulletGenerator[i].generatorDirection)*property.bulletGenerator[i].radius;
            bakePos.y = Mathf.Sin(Mathf.Deg2Rad*property.bulletGenerator[i].generatorDirection)*property.bulletGenerator[i].radius;
            em.bg_list[i].location=bakePos;
            em.bg_list[i].firstLoc=bakePos;
            //bake_bulletGenerator.direction = property.bulletGenerator[i].generatorDirection;
            //bake_bulletGenerator.radius = property.bulletGenerator[i].radius;
            for(int m=0; m<property.bulletGenerator[i].fireTime.Length;m++)
            {
                em.bg_list[i].fireTime.Add(property.bulletGenerator[i].fireTime[m]);
            }
            Debug.Log(i+" "+bakePos.x+" "+bakePos.y);
            Debug.Break();

            for(int j=0; j<property.bulletGenerator[i].pattern.Length;j++)
            {
                for(int k=0; k<property.bulletGenerator[i].pattern[j].parameter.Length;k++)
                {
                    bake_bulletParameter.range = property.bulletGenerator[i].pattern[j].parameter[k].range;
                    bake_bulletParameter.bulletDensity = property.bulletGenerator[i].pattern[j].parameter[k].bulletDirection;
                    bake_bulletParameter.bulletDirection = property.bulletGenerator[i].pattern[j].parameter[k].bulletDirection;
                    bake_bulletParameter.bulletSpeed = property.bulletGenerator[i].pattern[j].parameter[k].bulletSpeed;
                    bake_bulletParameter.bulletLabel = property.bulletGenerator[i].pattern[j].parameter[k].bulletLabel;
                    bake_bulletParameter.difficultyDensity = property.bulletGenerator[i].pattern[j].parameter[k].difficultyDensity;
                    bake_bulletParameter.difficultySpeed = property.bulletGenerator[i].pattern[j].parameter[k].difficultySpeed;
                    bake_bulletParameter.targetDirection = property.bulletGenerator[i].pattern[j].parameter[k].targetDirection;
                    bake_bulletParameter.isAimingPlayer = property.bulletGenerator[i].pattern[j].parameter[k].isAimingPlayer;
                    //em.bg_list[i].pattern[j].parameter.Add(bake_bulletParameter);
                    //Debug.Log(em.bg_list[i].pattern[j].parameter[k].bulletLabel);
                    switch(property.bulletGenerator[i].pattern[j].actions[k])
                    {
                        case BulletPatternProperty.PatternAction.Circular: bake_bulletPattern.act.Add(Actions._shootCircular); break;
                        case BulletPatternProperty.PatternAction.ShootAim: bake_bulletPattern.act.Add(Actions._shootAim); break;
                        case BulletPatternProperty.PatternAction.ShootFan: bake_bulletPattern.act.Add(Actions._shootFan); break;
                        
                    }
                    bake_bulletPattern.parameter.Add(bake_bulletParameter);
                }
                em.bg_list[i].pattern.Add(bake_bulletPattern);
            }
            
            Debug.Log(i+" "+bakePos.x+" "+bakePos.y);
            /*em.bg_list.Add(bake_bulletGenerator);
            p++;
            for(int q=0; q<em.bg_list.Count;q++)
            {
                Debug.Log("Looping Check "+q+" "+em.bg_list[q].location.x+" "+em.bg_list[q].location.y);            //Debug.Log("ASW "+ em.gameObject.name+" "+em.bg_list.Count);
            }*/
        }
        /*for(int i=0; i<property.bulletGenerator.Length;i++)
        {
            em.bulletGenerators[i]  = new BulletGenerator();
            BulletGenerator em_BG   = em.bulletGenerators[i];
            em_BG.parent            = enemy.transform;
            bake_loc.x              = property.bulletGenerator[i].radius*Mathf.Cos(Mathf.Deg2Rad*property.bulletGenerator[i].direction);
            bake_loc.y              = property.bulletGenerator[i].radius*Mathf.Sin(Mathf.Deg2Rad*property.bulletGenerator[i].direction);
            em_BG.loc               = bake_loc;
            em_BG.fireTime          = property.bulletGenerator[i].fireTime;
            BulletGeneratorProperty bgp = property.bulletGenerator[i];
            em_BG.pattern           = new BulletPattern[bgp.pattern.Length];
            
            for(int j=0; j<em_BG.pattern.Length;i++)
            {
                
                em_BG.pattern[j]    = new BulletPattern();
                BulletPattern bpatt = em_BG.pattern[j];
                BulletPatternProperty bpp = bgp.pattern[j];
                bpatt.parameter     = new BulletParameter[bpp.parameter.Length];
                
                bpatt.act           = new Actions.Action[bpp.actions.Length];
                for(int k=0; k<bpp.parameter.Length;k++)
                {
                    Debug.Log(k+" "+bpatt.parameter.Length+" "+gameObject.name);
                    Debug.Break();
                    bpatt.parameter[k]      = new BulletParameter();
                    bpatt.act[k]            = new Actions.Action();
                    bgbp                    = bpatt.parameter[k];
                    bgbp.range              = bpp.parameter[k].range;
                    bgbp.targetDirection    = bpp.parameter[k].targetDirection;
                    bgbp.bulletDirection    = bpp.parameter[k].bulletDirection;
                    bgbp.bulletDensity      = bpp.parameter[k].bulletDensity;
                    bgbp.bulletSpeed        = bpp.parameter[k].bulletSpeed;
                    bgbp.difficultyDensity  = bpp.parameter[k].difficultyDensity;
                    bgbp.difficultySpeed    = bpp.parameter[k].difficultySpeed;
                    bgbp.isAimingPlayer     = bpp.parameter[k].isAimingPlayer;
                    bgbp.bulletLabel        = bpp.parameter[k].bulletLabel;
                    switch(bpp.actions[k])
                    {
                        case BulletPatternProperty.PatternAction.Circular: bpatt.act[k] = Actions._shootCircular; break;
                        case BulletPatternProperty.PatternAction.ShootAim: bpatt.act[k] = Actions._shootAim; break;
                        case BulletPatternProperty.PatternAction.ShootFan: bpatt.act[k] = Actions._shootFan; break;
                    }
                }
                
            }
        }
        
        /*em.param.bulletDirection    = property.em.bulletDirection;
        em.param.bulletDensity      = property.em.bulletDensity;
        em.param.bulletSpeed        = property.em.bulletSpeed;
        em.param.difficultySpeed    = property.em.difficultySpeed;
        em.param.difficultyDensity  = property.em.difficultyDensity;
        em.param.range              = property.em.range;
        em.param.isAimingPlayer     = property.em.isAimingPlayer;
        em.param.targetDirection    = property.em.targetDirection;*/
        /*switch(property.act)
        {
            case GameController.Act.Circular: em.pattern = BulletParameter.Pattern.Circular; break;
            case GameController.Act.ShootAim: em.pattern = BulletParameter.Pattern.ShootAim; break;
            case GameController.Act.ShootFan: em.pattern = BulletParameter.Pattern.ShootFan; break;
        }*/
    }

    public void spawnPowerUp(Vector3 spawnPosition)
    {
        for(int b=0;b<bulletDictionary["PowerUp"].Count;b++)
        {
            if(!bulletDictionary["PowerUp"][b].activeInHierarchy)
            {
                bulletDictionary["PowerUp"][b].SetActive(true);
                bulletDictionary["PowerUp"][b].transform.localPosition=spawnPosition;
                return;
            }
        }
        GameObject obj = Instantiate(bulletDictionary["PowerUp"][0]);
        obj.transform.SetParent(playfieldAnchorTransform);
        obj.transform.localPosition=spawnPosition;
        obj.SetActive(true);
        bulletDictionary["PowerUp"].Add(obj);
        return;
    }

    public void shoot(string tag, Vector3 position, float rotate, float speed, Color col)
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
                return;
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
        return;
    }

    #endregion


    void Awake()
    {
        sharedOverseer = this; 
        Application.targetFrameRate=60;
    }

    void Start()
    {
        //bake_bulletGenerator=new BulletGenerator();
        //bake_bulletPattern = new BulletPattern();
        //bake_bulletParameter = new BulletParameter();
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
        border_cam[1] = new Vector3(0.9f, 0.5f, transform.localPosition.z);
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
            //Debug.Log("HUBLA");
        }

        
        //Debug.Log(Time.time);
    }
    
    
}
