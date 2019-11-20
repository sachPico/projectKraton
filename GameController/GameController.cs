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
        public List<BulletGeneratorProperty> bulletGenerator;
        public List<Timer> executeTimer;
        public float direction, speed, spawnPowerUpNumber, health;
    }
    [System.Serializable]
    public class BulletGeneratorProperty{
        public float direction, radius;
        public bool isGatling;
        [ConditionalHide("isGatling", true)]
        public Timer gatlingFireRate;

        public int timerIndex;
        public BulletParameter parameter;
        public enum PatternAction {ShootAim, ShootFan, ShootLayered, Circular};
        public PatternAction actions;
        public Color bulletColor;
    }

    public enum Difficulty{Easy=1, Normal, Hard};
    public enum Act{ShootAim, Circular, ShootFan};
    public Difficulty difficulty;

    public Score gameScore = new Score(0,0),powerStatus = new Score(8,1), powerCounter = new Score(99,2);

    public Dictionary<string, List<GameObject>> bulletDictionary;
    public List<Pool> bulletPools;
    public SpawnObject[] spawnObjects;
    
    Vector3 bakePos;//=new Vector3();
    Vector2 bakeSpawnPos;
    GameObject bakeGenerator;
    BulletGenerator bakeSubGenerator;
    public Vector3[] border_cam, border;
    public static double fieldBorder;

    public GameObject enemies, player, bulletGeneratorObject, scoreImages;
    public Camera mainCam;
    public Transform playfieldAnchorTransform;
    public Transform[] border_test;
    public static GameController sharedOverseer;

    public int playerLifes=2;
    //public uint score;

    //mending ga usah dijadiin satu class sendiri
    #region EnemyGenerationPattern

    public CameraControl camControl;
    private Transform camTransform;
    private int spawnCounter=0;
    float anchorDist;//, j=0;
    private Timer generateTimer;

    
    public void generateEnemy(SpawnObjectProperty property)
    {
        bakeSpawnPos.x = 0.3f + (0.4f * property.camPosition.x);
        bakeSpawnPos.y = property.camPosition.y;
        Vector3 toAnchor = mainCam.ViewportToWorldPoint(new Vector3(bakeSpawnPos.x,bakeSpawnPos.y,Mathf.Abs(mainCam.transform.localPosition.z)));
        GameObject enemy = Instantiate(property.obj, toAnchor, Quaternion.identity,playfieldAnchorTransform);
        enemy.transform.localRotation = Quaternion.Euler(0,0,property.direction);
        EnemyMove em = enemy.GetComponent<EnemyMove>();
        em.health                   = property.health;
        em.spawnPowerUp             = property.spawnPowerUpNumber;
        em.speed                    = property.speed;
        em.generatorTimer           = property.executeTimer;
        for(int i=0; i<property.bulletGenerator.Count;i++)
        {
            bakePos.x=Mathf.Cos(property.bulletGenerator[i].direction*Mathf.Deg2Rad)*property.bulletGenerator[i].radius;
            bakePos.y=Mathf.Sin(property.bulletGenerator[i].direction*Mathf.Deg2Rad)*property.bulletGenerator[i].radius;
            bakeGenerator=Instantiate(bulletGeneratorObject,em.transform,false);
            bakeGenerator.transform.localRotation = Quaternion.identity;
            bakeGenerator.transform.localPosition = bakePos;
            em.bg_list.Add(bakeGenerator.AddComponent(typeof(BulletGenerator)) as BulletGenerator);
            
            em.bg_list[i].firstLoc=bakePos;
            em.bg_list[i].parameter = property.bulletGenerator[i].parameter;
            em.bg_list[i].isShoot = true;
            em.bg_list[i].bulletColor = property.bulletGenerator[i].bulletColor;
            switch(property.bulletGenerator[i].actions)
            {
                case GameController.BulletGeneratorProperty.PatternAction.Circular:
                    em.bg_list[i].act = Actions._shootCircular; break;
                case GameController.BulletGeneratorProperty.PatternAction.ShootAim:
                    em.bg_list[i].act = Actions._shootDirection; break;
                case GameController.BulletGeneratorProperty.PatternAction.ShootFan:
                    em.bg_list[i].act = Actions._shootFan; break;
                case GameController.BulletGeneratorProperty.PatternAction.ShootLayered:
                    em.bg_list[i].act = Actions._shootLayered; break;
            }
            if(property.bulletGenerator[i].isGatling)
            {
                bakeSubGenerator = Instantiate(bulletGeneratorObject, em.bg_list[i].transform, false).AddComponent(typeof(BulletGenerator)) as BulletGenerator;
                bakeSubGenerator.gatlingTimer = property.bulletGenerator[i].gatlingFireRate;
                bakeSubGenerator.gameObject.name="SubGenerator "+i;
                //bakeSubGenerator.gatlingTimer.isResettable = property.bulletGenerator[i].gatlingFireRate.isResettable;
                //bakeSubGenerator.gatlingTimer.iterations = property.bulletGenerator[i].gatlingFireRate.maxIterations;
                bakeSubGenerator.gatlingTimer.ExecuteEvent += em.bg_list[i].GeneratorShoot;
                em.generatorTimer[property.bulletGenerator[i].timerIndex].ExecuteEvent += bakeSubGenerator.gatlingTimer.resetTimer;
            }
            else
            {
                em.generatorTimer[property.bulletGenerator[i].timerIndex].ExecuteEvent += em.bg_list[i].GeneratorShoot;
            }
            //Debug.Break();
        }
        
        //Code to set the last timer in enemy object to execute timer reset to all timers in one enemy
        for(int i=0; i<em.generatorTimer.Count;i++)
        {
            em.generatorTimer[em.generatorTimer.Count-1].ExecuteEvent += em.generatorTimer[i].resetTimer;
        }
        //Debug.Break();
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
                SpriteRenderer sr = bulletDictionary[tag][i].GetComponent<SpriteRenderer>();
                sr.color = col;
                sr.sortingOrder = i;
                spd.speed = speed;
                return bulletDictionary[tag][i];
            }
        }
        GameObject obj = Instantiate(bulletDictionary[tag][0]);
        SpriteRenderer newsr = obj.GetComponent<SpriteRenderer>();
        newsr.color = col;
        newsr.sortingOrder = bulletDictionary[tag].Count+1;
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
        camControl.resetOrigin = mainCam.transform.localPosition.x;
    }
    void Awake()
    {
        sharedOverseer = this; 
        Application.targetFrameRate=60;
    }

    void Start()
    {
        //PRINT TITIK BATAS DI DIMENSI WORLD
        //MULAI OBJECT POOL
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
        //AKHIR OBJECT POOL

        //ATUR POSISI BATAS HORIZONTAL
        border_cam[0] = new Vector3(0.3f, 0.5f, transform.localPosition.z); //KIRI TENGAH
        border_cam[1] = new Vector3(.7f, 0.5f, transform.localPosition.z);  //KANAN TENGAH

        //ATUR POSISI BATAS VERTIKAL
        border_cam[2] = new Vector3(0.5f, 0.1f, transform.localPosition.z); //TENGAH BAWAH
        border_cam[3] = new Vector3(0.5f, 0.9f, transform.localPosition.z); //TENGAH ATAS

        border[0]=new Vector3();
        border[1]=new Vector3();
        border[2]=new Vector3();
        border[3]=new Vector3();
        for(int i=0; i<4; i++)
        {
            border[i]=new Vector3();
        }
        camTransform=mainCam.transform;
        camControl = mainCam.gameObject.GetComponent<CameraControl>();
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
//Total baris program yang kau buat: 1442