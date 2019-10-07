using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject enemies;
    public Camera mainCam;
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
                GameController.sharedOverseer.generateEnemy(sop[i].camPosition, sop[i].direction);
            }
        }
    }
    [System.Serializable]
    public class SpawnObjectProperty{
        public GameObject obj;
        public Vector2 camPosition;
        public float direction, speed;
    }
    public Transform playfieldAnchorTransform;
    public List<Pool> bulletPools;
    public Dictionary<string, List<GameObject>> bulletDictionary;
    public static GameController sharedOverseer;
    public SpawnObject[] spawnObjects;
    public Transform border_test;
    //mending ga usah dijadiin satu class sendiri
    #region EnemyGenerationPattern

    private int i=0, j=0;
    public Vector3[] border_cam, border;

    private Timer generateTimer;
    //public Timer[] timerList;
    //public Vector2[] locationList;
    //public float[] rotationList, timerList;

    public void generateEnemy(Vector2 location, float rotation)
    {
        Vector3 toAnchor = mainCam.ViewportToWorldPoint(new Vector3(location.x,location.y,Mathf.Abs(mainCam.transform.localPosition.z)));
        GameObject enemy = Instantiate(enemies, toAnchor, Quaternion.identity,playfieldAnchorTransform);
        enemy.transform.localRotation = Quaternion.Euler(0,0,rotation);
        EnemyMove em = enemy.GetComponent<EnemyMove>();
        em.speed=10f;
        //em.playfieldAnchor=playfieldAnchorTransform;
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
        //pooledObjects.Add(obj);
    }

    #endregion


    void Awake()
    {
        sharedOverseer = this; 
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
        //Debug.Log(spawnObjects.Length);
        //generateTimer = new Timer(timerList[0]);
        //Horizontal side border
        /*border_cam[0] = new Vector3(0.1f, 0.1f, transform.localPosition.z);
        border_cam[1] = new Vector3(0.9f, 0.1f, transform.localPosition.z);
        //Vertical side border
        border_cam[2] = new Vector3(0.1f, 0.9f, transform.localPosition.z);
        border_cam[3] = new Vector3(0.9f, 0.9f, transform.localPosition.z);
        border[0]=new Vector3();
        border[1]=new Vector3();
        border[2]=new Vector3();
        border[3]=new Vector3();*/
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
            //Debug.Log(j+", "+border_cam[j].z);
            border[j]=mainCam.ViewportToWorldPoint(border_cam[j]);
            //Debug.Log("HUBLA");
        }
        border_test.position=border[0];
    }

    
}
