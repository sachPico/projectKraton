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
        public SpawnObjectProperty[] sop;
        public void Generate()
        {
            for(int i=0; i<sop.Length; i++)
            {
                GameController.sharedOverseer.generateEnemy(sop[i]);
            }
        }
    }
    [System.Serializable]
    public class SpawnObjectProperty{
        public GameObject obj;
        public Vector2 camPosition;
        public float direction, speed, spawnPowerUpNumber, bulletSpeed;
        public float difficultyIntensity, difficultySpeedIntensity, bulletDensity, health;
        public Act act;
    }
    public enum Act{ShootAim, Circular};
    public enum Difficulty{Easy=1, Normal, Hard};
    public Difficulty difficulty;

    public Dictionary<string, List<GameObject>> bulletDictionary;
    public List<Pool> bulletPools;
    public SpawnObject[] spawnObjects;
    public Vector3[] border_cam, border;

    public GameObject enemies, player;
    public Camera mainCam;
    public Transform playfieldAnchorTransform;
    public Transform[] border_test;
    public static GameController sharedOverseer;

    //mending ga usah dijadiin satu class sendiri
    #region EnemyGenerationPattern

    private int i=0, j=0;

    private Timer generateTimer;

    public void generateEnemy(SpawnObjectProperty property)
    {
        Vector3 toAnchor = mainCam.ViewportToWorldPoint(new Vector3(property.camPosition.x,property.camPosition.y,Mathf.Abs(mainCam.transform.localPosition.z)));
        GameObject enemy = Instantiate(property.obj, toAnchor, Quaternion.identity,playfieldAnchorTransform);
        enemy.transform.localRotation = Quaternion.Euler(0,0,property.direction);
        EnemyMove em = enemy.GetComponent<EnemyMove>();
        em.speed=property.speed;
        em.difficultyDensityIntensity = property.difficultyIntensity;
        em.spawnPowerUp=property.spawnPowerUpNumber;
        em.bulletDensity=property.bulletDensity;
        em.bulletSpeed=property.bulletSpeed;
        em.difficultySpeedIntensity=property.difficultySpeedIntensity;
        em.health=property.health;
        switch(property.act)
        {
            case Act.Circular: em.pattern = EnemyMove.Pattern.Circular; break;
            case Act.ShootAim: em.pattern = EnemyMove.Pattern.ShootAim; break;
        }
    }

    public void spawnPowerUp(Vector3 spawnPosition)
    {
        for(int i=0;i<bulletDictionary["PowerUp"].Count;i++)
        {
            if(!bulletDictionary["PowerUp"][i].activeInHierarchy)
            {
                bulletDictionary["PowerUp"][i].SetActive(true);
                bulletDictionary["PowerUp"][i].transform.localPosition=spawnPosition;
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
            border[j]=new Vector3();

        }
    }

    void Update()
    {
        if(i<spawnObjects.Length)
        {
            if(spawnObjects[i].timer.timerCount())
            {
                spawnObjects[i].Generate();
                i++;
            }
        }
        for(j=0;j<4;j++)
        {
            border_cam[j].z=Mathf.Abs(mainCam.transform.localPosition.z);
            border[j]=mainCam.ViewportToWorldPoint(border_cam[j]);
            border_test[j].position=border[j];
            //Debug.Log("HUBLA");
        }

        
        Debug.Log(Time.time);
    }
    
    
}
